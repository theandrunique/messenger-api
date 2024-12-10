# users

CREATE TABLE users (
    id uuid,
    username text,
    username_updated_at timestamp,
    password_hash blob,
    password_updated_at blob,
    terminate_sessions text,
    bio text,
    global_name text,
    is_active boolean,
    created_at timestamp,
    key text,
    two_factor_authentication boolean,
    email text,
    is_email_verified boolean,
    email_updated_at timestamp,
    images set<text>,

    PRIMARY KEY (id)
);

## Queries

`SELECT * FROM users WHERE Id = {};`
`SELECT * FROM users WHERE Username = {};`
`SELECT * FROM users WHERE Email = {};`


# channels

CREATE TABLE channels_by_id (
    channel_id timeuuid,
    channel_type text,
    owner_id uuid,
    title text,
    image text,
    last_message_id timeuuid,

    PRIMARY KEY (id)
);

CREATE TABLE channel_participants (
    channel_id timeuuid,
    user_id uuid,
    PRIMARY KEY (channel_id, user_id)
);


CREATE TABLE channels_by_user_id (
    user_id uuid,
    channel_type text,
    channel_id timeuuid,

    PRIMARY KEY (user_id, channel_type, channel_id)
);

CREATE TABLE channels_private (
    user1_id uuid, -- Lower UUID
    user2_id uuid, -- Higher UUID
    channel_id timeuuid,

    PRIMARY KEY ((user1_id, user2_id))
);

## Queries

`SELECT * FROM channels_by_id WHERE Id = {}`
`SELECT * FROM channels_by_user_id WHERE UserId = {}`
`SELECT * FROM channels_by_user_id WHERE UserId = {} AND ChannelType = {}`
`SELECT * FROM channels_private WHERE User1Id = {} AND User2Id = {}`

## Use Cases

1. Get all channels user participates in
Data: user_id

- Get all channel_id from channels_by_user_id where user_id = user_id
- Get all channels from channels_by_id where channel_id in channel_id
- For channels which channel_type is private then get user info from channels_private
- return

2. Create Private channel
Data: user1_id, user2_id

- Check that chat are not already exists by query channels_private
- Create new if was not found
- If new was created then add records to channels_by_id and channels_by_user_id
- return

3. Create SavedMessages channel

- Check that chat are not already exists
- Create new if was not found
- If new was created then add records to channels_by_id and channels_by_user_id
- return




# Message

CREATE TABLE messages (
    channel_id timeuuid,
    bucket int, -- Для исскуственного шардирования
    message_id timeuuid,
    sender_id uuid,
    content text,
    sent_at timestamp,
    updated_at timestamp,
    reply_to timeuuid,
    attachments list<frozen<attachent>>,

    PRIMARY KEY((channel_id, bucket), message_id)
);

CREATE TYPE attachent (
    channel_id uuid,
    message_id timeuuid,
    id uuid,
    filename text,
    uploaded_filename text,
    content_type text,
    size bigint,
    pre_signed_url text
    pre_signed_url_expires_at timestamp,
    Placeholder text,
    duration_secs double,
    waveform text,
    is_spoiler boolean,
);


# Attachment

CREATE TABLE attachments (
    channel_id timeuuid,
    message_id timeuuid,
    id uuid,
    filename text,
    uploaded_filename text,
    content_type text,
    size bigint,
    pre_signed_url text
    pre_signed_url_expires_at timestamp,
    placeholder text,
    duration_secs double,
    waveform text,
    is_spoiler boolean,

    PRIMARY KEY ((channel_id), message_id)
);



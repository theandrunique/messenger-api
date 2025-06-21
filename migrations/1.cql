CREATE KEYSPACE messenger
WITH replication = {'class': 'NetworkTopologyStrategy', 'replication_factor': 1};

CREATE KEYSPACE auth
WITH replication = {'class': 'NetworkTopologyStrategy', 'replication_factor': 1}
AND tablets = {'enabled': false};

USE messenger;

-- TYPES

CREATE TYPE messenger.message_info (
    id bigint,
    author_id bigint,
    target_user_id bigint,
    content text,
    timestamp timestamp,
    edited_timestamp timestamp,
    attachments_count int,
    type int,
    metadata text
);

-- TABLES

CREATE TABLE messenger.attachments_by_message_id (
    channel_id bigint,
    attachment_id bigint,
    message_id bigint,
    content_type text,
    duration_secs float,
    filename text,
    is_spoiler boolean,
    placeholder text,
    presigned_url text,
    presigned_url_expires_timestamp timestamp,
    size bigint,
    waveform text,
    timestamp timestamp,

    PRIMARY KEY ((channel_id), message_id, attachment_id)
);

CREATE MATERIALIZED VIEW attachments_by_id AS
    SELECT
        channel_id,
        attachment_id,
        message_id,
        content_type,
        duration_secs,
        filename,
        is_spoiler,
        placeholder,
        presigned_url,
        presigned_url_expires_timestamp,
        size,
        waveform,
        timestamp
    FROM messenger.attachments_by_message_id
    WHERE
        channel_id IS NOT NULL
        AND attachment_id IS NOT NULL
        AND message_id IS NOT NULL
    PRIMARY KEY (channel_id, attachment_id, message_id)
    WITH CLUSTERING ORDER BY (attachment_id DESC);

CREATE TABLE messenger.channel_users_by_user_id (
    user_id bigint,
    channel_id bigint,
    last_read_message_id bigint,
    username text,
    global_name text,
    image text,
    permission_overwrites bigint,
    is_leave boolean,

    PRIMARY KEY (user_id, channel_id)
);

CREATE MATERIALIZED VIEW messenger.channel_users_by_channel_id AS
    SELECT
        user_id,
        channel_id,
        last_read_message_id,
        username,
        global_name,
        image,
        permission_overwrites,
        is_leave
    FROM messenger.channel_users_by_user_id
    WHERE
        channel_id IS NOT NULL
        AND user_id IS NOT NULL
    PRIMARY KEY(channel_id, user_id);

CREATE TABLE messenger.channels_by_id (
    channel_id bigint,
    type int,
    name text,
    owner_id bigint,
    image text,
    last_message message_info,
    last_message_timestamp timestamp,
    permission_overwrites bigint,

    PRIMARY KEY ((channel_id))
);

CREATE TABLE messenger.messages (
    channel_id bigint,
    message_id bigint,
    author_id bigint,
    target_user_id bigint,
    content text,
    timestamp timestamp,
    edited_timestamp timestamp,
    pinned boolean,
    type int,
    referenced_message_id bigint,
    metadata text,

    PRIMARY KEY ((channel_id), message_id)
) WITH CLUSTERING ORDER BY (message_id DESC);

CREATE TABLE messenger.message_acks (
    channel_id bigint,
    user_id bigint,
    last_read_message_id bigint,
    timestamp timestamp,

    PRIMARY KEY ((channel_id), last_read_message_id, user_id)
) WITH CLUSTERING ORDER BY (last_read_message_id DESC);

CREATE TABLE messenger.private_channels (
    user_id1 bigint,
    user_id2 bigint,
    channel_id bigint,

    PRIMARY KEY ((user_id1, user_id2))
);

CREATE TABLE auth.sessions (
    user_id bigint,
    session_id bigint,
    client_name text,
    timestamp timestamp,
    device_name text,
    last_used_timestamp timestamp,
    location text,
    token_id uuid,

    PRIMARY KEY ((user_id), session_id)
);

CREATE INDEX sessions_token_id_idx ON auth.sessions(token_id);


CREATE TABLE auth.users (
    user_id bigint,
    bio text,
    timestamp timestamp,
    sessions_lifetime int,
    email text,
    email_updated_timestamp timestamp,
    global_name text,
    is_active boolean,
    is_email_verified boolean,
    totp_key blob,
    password_hash text,
    password_updated_timestamp timestamp,
    two_factor_authentication boolean,
    username text,
    username_updated_timestamp timestamp,
    image text,

    PRIMARY KEY ((user_id))
);


CREATE TABLE auth.users_by_username (
    username text,
    user_id bigint,

    PRIMARY KEY ((username))
);

CREATE TABLE auth.users_by_email (
    email text,
    user_id bigint,

    PRIMARY KEY ((email))
);


CREATE TABLE auth.verification_codes (
    identifier text, -- email, phone, etc
    scenario int,   -- (enum) reset_password, verify_email, etc
    code_hash text,
    timestamp timestamp,
    expires_timestamp timestamp,
    attempts int,
    PRIMARY KEY ((identifier, scenario))
);

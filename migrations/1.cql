CREATE KEYSPACE messenger
WITH replication = {'class': 'NetworkTopologyStrategy', 'replication_factor': 1};

USE messenger;

-- TYPES

CREATE TYPE messenger.messageinfo (
    id bigint,
    authorid bigint,
    authorusername text,
    authorglobalname text,
    content text,
    timestamp timestamp,
    editedtimestamp timestamp,
    attachmentscount int,
    type int,

);

CREATE TYPE messenger.image (
    key text
);

-- TABLES

CREATE TABLE messenger.attachments (
    channelid bigint,
    messageid bigint,
    id bigint,
    contenttype text,
    durationsecs float,
    filename text,
    isspoiler boolean,
    placeholder text,
    presignedurl text,
    presignedurlexpirestimestamp timestamp,
    size bigint,
    uploadfilename text,
    waveform text,

    PRIMARY KEY (channelid, messageid, id)
);

CREATE TABLE messenger.channel_users_by_user_id (
    userid bigint,
    channelid bigint,
    readat bigint,
    username text,
    globalname text,
    image frozen<image>,
    permissions bigint,

    PRIMARY KEY (userid, channelid)
);

CREATE MATERIALIZED VIEW messenger.channel_users_by_channel_id AS
    SELECT userid, channelid, readat, username, globalname, image, permissions
    FROM messenger.channel_users_by_user_id
    WHERE channelid IS NOT NULL AND userid IS NOT NULL
    PRIMARY KEY(channelid, userid);

CREATE TABLE messenger.channels_by_id (
    channelid bigint,
    channeltype int,
    image frozen<image>,
    lastmessage messageinfo,
    lastmessagetimestamp timestamp,
    ownerid bigint,
    title text,

    PRIMARY KEY (channelid)
);

CREATE TABLE messenger.messages (
    channelid bigint,
    id bigint,
    authorid bigint,
    content text,
    timestamp timestamp,
    editedtimestamp timestamp,
    pinned boolean,
    type int,

    PRIMARY KEY (channelid, id)
);

CREATE TABLE messenger.private_channels (
    userid1 bigint,
    userid2 bigint,
    channelid bigint,

    PRIMARY KEY ((userid1, userid2))
);

CREATE TABLE messenger.sessions (
    userid bigint,
    id bigint,
    clientname text,
    timestamp timestamp,
    devicename text,
    lastusedtimestamp timestamp,
    location text,
    tokenid uuid,

    PRIMARY KEY (userid, id)
);

CREATE INDEX sessions_tokenid_idx ON messenger.sessions(tokenid);


CREATE TABLE messenger.users (
    id bigint,
    bio text,
    timestamp timestamp,
    terminatesessions int,
    email text,
    emailupdatedtimestamp timestamp,
    globalname text,
    isactive boolean,
    isemailverified boolean,
    totpkey blob,
    passwordhash text,
    passwordupdatedtimestamp timestamp,
    twofactorauthentication boolean,
    username text,
    usernameupdatedtimestamp timestamp,
    image frozen<image>,

    PRIMARY KEY (id)
);

CREATE INDEX users_email_idx ON messenger.users(email);

CREATE INDEX users_username_idx ON messenger.users(username);


CREATE TABLE messenger.verification_codes (
    identifier text, -- email, phone, etc
    scenario int,   -- (enum) reset_password, verify_email, etc
    codehash text,
    timestamp timestamp,
    expirestimestamp timestamp,
    attempts int,
    PRIMARY KEY ((identifier, scenario))
);

CREATE KEYSPACE messenger
WITH replication = {'class': 'NetworkTopologyStrategy', 'replication_factor': 1};

USE messenger;

-- TYPES

CREATE TYPE messenger.messageinfo (
    id bigint,
    authorid bigint,
    targetuserid bigint,
    content text,
    timestamp timestamp,
    editedtimestamp timestamp,
    attachmentscount int,
    type int,
    metadata text
);

-- TABLES

CREATE TABLE messenger.attachments_by_messageid (
    channelid bigint,
    id bigint,
    messageid bigint,
    contenttype text,
    durationsecs float,
    filename text,
    isspoiler boolean,
    placeholder text,
    presignedurl text,
    presignedurlexpirestimestamp timestamp,
    size bigint,
    waveform text,
    timestamp timestamp,

    PRIMARY KEY ((channelid), messageid, id)
);

CREATE MATERIALIZED VIEW attachments_by_id AS
    SELECT
        channelid,
        id,
        messageid,
        contenttype,
        durationsecs,
        filename,
        isspoiler,
        placeholder,
        presignedurl,
        presignedurlexpirestimestamp,
        size,
        waveform,
        timestamp
    FROM messenger.attachments_by_messageid
    WHERE
        channelid IS NOT NULL
        AND id IS NOT NULL
        AND messageid IS NOT NULL
    PRIMARY KEY (channelid, id, messageid)
    WITH CLUSTERING ORDER BY (id DESC);

CREATE TABLE messenger.channel_users_by_user_id (
    userid bigint,
    channelid bigint,
    lastreadmessageid bigint,
    username text,
    globalname text,
    image text,
    permissionoverwrites bigint,
    isleave boolean,

    PRIMARY KEY (userid, channelid)
);

CREATE MATERIALIZED VIEW messenger.channel_users_by_channel_id AS
    SELECT userid, channelid, lastreadmessageid, username, globalname, image, permissionoverwrites, isleave
    FROM messenger.channel_users_by_user_id
    WHERE channelid IS NOT NULL AND userid IS NOT NULL
    PRIMARY KEY(channelid, userid);

CREATE TABLE messenger.channels_by_id (
    channelid bigint,
    channeltype int,
    name text,
    ownerid bigint,
    image text,
    lastmessage messageinfo,
    lastmessagetimestamp timestamp,
    permissionoverwrites bigint,

    PRIMARY KEY ((channelid))
);

CREATE TABLE messenger.messages (
    channelid bigint,
    id bigint,
    authorid bigint,
    targetuserid bigint,
    content text,
    timestamp timestamp,
    editedtimestamp timestamp,
    pinned boolean,
    type int,
    metadata text,

    PRIMARY KEY ((channelid), id)
) WITH CLUSTERING ORDER BY (id DESC);

CREATE TABLE messenger.message_acks (
    channelid bigint,
    userid bigint,
    lastreadmessageid bigint,
    timestamp timestamp,

    PRIMARY KEY ((channelid), lastreadmessageid, userid)
) WITH CLUSTERING ORDER BY (lastreadmessageid DESC);

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

    PRIMARY KEY ((userid), id)
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
    image text,

    PRIMARY KEY ((id))
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

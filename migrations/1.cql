CREATE KEYSPACE messenger
WITH replication = {'class': 'NetworkTopologyStrategy', 'replication_factor': 1};

USE messenger;

-- TYPES

CREATE TYPE messenger.messageinfo (
    authorglobalname text,
    content text,
    sentat timestamp,
    attachmentscount int
);

CREATE TYPE messenger.image (
    key text
);

CREATE TYPE messenger.messagesenderinfo (
    id uuid,
    username text,
    globalname text,
    images list<frozen<image>>
);

-- TABLES

CREATE TABLE messenger.attachments (
    channelid uuid,
    messageid uuid,
    id bigint,
    contenttype text,
    durationsecs float,
    filename text,
    isspoiler boolean,
    placeholder text,
    presignedurl text,
    presignedurlexpiresat timestamp,
    size bigint,
    uploadedfilename text,
    waveform text,
    PRIMARY KEY (channelid, messageid, id)
);

CREATE TABLE messenger.channel_users_by_user_id(
    userid uuid,
    channelid uuid,
    readat timeuuid,
    username text,
    globalname text,
    images list<frozen<image>>,
    PRIMARY KEY (userid)
);

CREATE MATERIALIZED VIEW messenger.channel_users_by_channel_id AS
    SELECT userid, channelid, username, globalname, images FROM messenger.channel_users_by_user_id
    WHERE channelid IS NOT NULL AND userid IS NOT NULL
    PRIMARY KEY(channelid, userid);

CREATE TABLE messenger.channels_by_id (
    channelid uuid,
    channeltype int,
    image image,
    lastmessage messageinfo,
    lastmessageat timestamp,
    ownerid uuid,
    title text,
    PRIMARY KEY (channelid)
);

CREATE TABLE messenger.saved_messages_channel(
    userid uuid,
    channelid uuid,
    PRIMARY KEY(userid)
);

CREATE TABLE messenger.messages (
    channelid uuid,
    id timeuuid,
    author frozen<messagesenderinfo>,
    content text,
    replyto uuid,
    senderid uuid,
    sentat timestamp,
    updatedat timestamp,
    PRIMARY KEY (channelid, id)
);

CREATE TABLE messenger.private_channels (
    userid1 uuid,
    userid2 uuid,
    channelid uuid,
    PRIMARY KEY ((userid1, userid2))
);

CREATE TABLE messenger.sessions (
    userid uuid,
    id uuid,
    clientname text,
    createdat timestamp,
    devicename text,
    lastusedat timestamp,
    location text,
    tokenid uuid,
    PRIMARY KEY (userid, id)
);

CREATE INDEX sessions_tokenid_idx ON messenger.sessions(tokenid);


CREATE TABLE messenger.users (
    id uuid,
    bio text,
    createdat timestamp,
    email text,
    emailupdatedat timestamp,
    globalname text,
    isactive boolean,
    isemailverified boolean,
    key blob,
    passwordhash text,
    passwordupdatedat timestamp,
    terminatesessions text,
    twofactorauthentication boolean,
    username text,
    usernameupdatedat timestamp,
    PRIMARY KEY (id)
);

CREATE INDEX users_email_idx ON messenger.users(email);

CREATE INDEX users_username_idx ON messenger.users(username);

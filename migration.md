keyspace = "messenger"

# users

id: uuid
username: text
username_updated_at: timestamp
password_hash: blob
password_updated_at: blob
terminate_sessions: Literal[Week, Month, Month3, Month6, Year]
bio: text
global_name: text
is_active: boolean
created_at: timestamp
key: text
two_factor_authentication: boolean
email: text
is_email_verified: boolean
email_updated_at: timestamp
images: set<text>

## Queries

`SELECT * FROM users WHERE Id = {};`
`SELECT * FROM users WHERE Username = {};`
`SELECT * FROM users WHERE Email = {};`


# channels

id: uuid
owner_id: uuid
title: text
image: text
channel_type: Literal[Private, Group]
last_message_id: long

List<Message> Messages
List<User> Members
List<User> Admins
List<PinnedMessageId> PinnedMessageIds

## Queries

`SELECT * FROM channels WHERE Id = {}`
`SELECT * FROM channels WHERE  = {}`
`SELECT * FROM channels WHERE Id = {}`


# Message

long Id
Guid ChannelId
Guid SenderId
string Text
DateTime SentAt
DateTime? UpdatedAt
long? ReplyTo

List<UserReaction> Reactions
List<Attachment> Attachments


# Attachment

long Id
long MessageId
Guid ChannelId
string Filename
string UploadedFilename
string ContentType
long Size
string PreSignedUrl
DateTime PreSignedUrlExpiresAt
string? Placeholder
float? DurationSecs
string? Waveform
bool IsSpoiler


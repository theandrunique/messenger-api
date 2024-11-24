# Message structure
| Filed       | Type              | Description                               |
| ----------- | ----------------- | ----------------------------------------- |
| Id          | long              | Id of the message                         |
| ChannelId   | Guid              | Id of the channel the message was sent in |
| SenderId    | Guid              | Id of the sender                          |
| Text        | string            | Text of the message                       |
| SentAt      | DateTime          |                                           |
| UpdatedAt?  | DateTime          |                                           |
| ReplyTo?    | long              |                                           |
| Attachments | array[Attachment] |                                           |
| Reactions   | array[Reaction]   |                                           |

# Message Type

| Value | Name                   | Description                                        | Rendered Content                              | Deletable |
| ----- | ---------------------- | -------------------------------------------------- | --------------------------------------------- | --------- |
| 0     | DEFAULT                | A default message                                  | "{content}"                                   | true      |
| 1     | MEMBER_ADD             | A message sent when a user is added to the channel | "{member}" added {mentions[0]} to the channel | false     |
| 2     | MEMBER_REMOVE          |                                                    |                                               | false     |
| 3     | CHANNEL_NAME_CHANGE    |                                                    |                                               | false     |
| 4     | CHANNEL_ICON_CHANGE    |                                                    |                                               | false     |
| 5     | REPLY                  |                                                    |                                               | true      |
| 6     | AUTO_MODERATION_ACTION |                                                    |                                               | true      |
| 7     | POLL_RESULT            |                                                    |                                               | true      |

# Example Message
```json
{
	"id": 213123,
	"type": "DEFAULT",
	"text": "lol",
	"channelId": uuid4(),
	"sender": {
		"id": uuid4(),
		"username": "bob"
		"globalName": "Bob",
		"images": []
	},
	"attachments": [],
	"mentions": [],
	"pinned": false,
	"timestamp": "",
	"editedTimestamp": null,
	"reactions": [
		{
		
		}
	]
}
```

## Attachment Object
#### Attachment Structure
| Field            | Type   | Description |
| ---------------- | ------ | ----------- |
| Id               | long   |             |
| filename         | string |             |
| uploadedFilename | string |             |
| contentType      | string |             |
| size             | long   |             |
| url              | string |             |
| height?          | int    |             |
| width?           | int    |             |
| placeholder?     | string |             |
| durationSecs?    | float  |             |
| waveform?        | string |             |
| isSpoiler?       | bool   |             |
|                  |        |             |
# Endpoints
#### Get Messages
GET `/channels/{channel.id}/messages`
##### Query String Params
| Filed  | Type | Description |
| ------ | ---- | ----------- |
| offset | int  |             |
| limit  | int  |             |
#### Search Messages
GET `/channels/{channel.id}/messages/search`

| Field     | Type   | Description |
| --------- | ------ | ----------- |
| limit     | int    |             |
| offset    | int    |             |
| content?  | string |             |
| senderId? | uuid4  |             |
#### Get Message
GET `/channels/{channel.id}/messages/{message.id}`

#### Create Message
POST `/channels/{channel.id}/messages`

#### Create Attachments
POST `/channels/{channel.id}/attachments`
Create attachment URLs for upload the file directly to S3 bucket.
##### JSON Params
| Filed | Type                     | Description                                               |
| ----- | ------------------------ | --------------------------------------------------------- |
| files | array[upload attachment] | The files to create a URL for, contains the name and size |
##### Upload Attachment Structure
| FIled    | Type   | Description                                          |
| -------- | ------ | ---------------------------------------------------- |
| id?      | string | The ID o the attachment to reference in the response |
| filename | string |                                                      |
| fileSize | long   | The size of the file in bytes                        |
##### Cloud Attachment Structure
| Field          | Type   | Description                                                 |
| -------------- | ------ | ----------------------------------------------------------- |
| id?            | string | The ID of the attachment upload, if provided in the request |
| uploadUrl      | string | The URL to upload file to                                   |
| uploadFilename | string | The name of the uploaded file                               |
##### Example Response
```json
{
	"attachments": [
		{
			"id": "5",
			"uploadUrl": "https://...",
			"uploadFilename": "attachments/..."
		}
	]
}
```

#### Delete Attachment
DELETE `/attachments/{cloudAttachment.uploadFilename}`
Deletes an attachment from S3 storage.
#### Acknowledge Message
POST `/channels/{channel.id}/messages/{message.id}/ack`
Marks a message as read for the current user.
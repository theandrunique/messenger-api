```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "username": "admin",
    "emails": [
        {
            "data": "admin@gmail.com",
            "isVerified": true,
            "isPublic": true,
            "addedAt": "2022-01-01T00:00:00.000Z"
        }
    ],
    "phones": [
        {
            "data": "+1234567890",
            "isVerified": false,
            "addedAt": "2022-01-01T00:00:00.000Z"
        }
    ],
    "usernameUpdateAd": "2022-01-01T00:00:00.000Z",
    "passwordUpdateAt": "2022-01-01T00:00:00.000Z",
    "terminateSessions": "Week",
    "bio": null,
    "profileImage": null,
    "globalName": "Admin",
    "createdAt": "2022-01-01T00:00:00.000Z",
    "twoFactorAuthentication": false
}
```

```json
{
    "profilePhotos": [
        {
            "id": "3125",
            "userId": "00000000-0000-0000-0000-000000000000",
            "isPublic": true,
            "file": {
                "url": "https://example.com/image.png",
                "size": 1024,
                "uploadedAt": "2022-01-01T00:00:00.000Z"
            }
        }
    ]
}
```

```json
{
    "sessions": [
        {
            "id": "00000000-0000-0000-0000-000000000000",
            "deviceName": "My Device",
            "clientName": "browser",
            "location": "USA NY",
            "lastActivity": "2022-01-01T00:00:00.000Z",
            "createdAt": "2022-01-01T00:00:00.000Z",
            "ipAddress": "153.123.123.123"
        }
    ]
}
```

```json
{
    "chats": [
        {
            "id": 7324,
            "ownerId": "00000000-0000-0000-0000-000000000000",
            "tittle": "My Chat",
            "chatPhoto": {
                "url": "https://example.com/image.png",
                "size": 1024,
                "uploadedAt": "2022-01-01T00:00:00.000Z"
            },
            "type": "Group",
            "memberIds": [
                "00000000-0000-0000-0000-000000000000"
            ],
            "adminIds": [
                "00000000-0000-0000-0000-000000000000"
            ],
            "lastMessage": {
                "id" 43,
                "senderId": "00000000-0000-0000-0000-000000000000",
                "text": "Hello, gays!",
                "sentAt": "2022-01-01T00:00:00.000Z",
                "updatedAt": "2022-01-01T00:00:00.000Z",
                "replyToId": 40,
                "attachments": [
                    {
                        "type": "Video",
                        "url": "https://example.com/image.png",
                        "size": 1024,
                        "uploadedAt": "2022-01-01T00:00:00.000Z"
                    }
                ],
                "reactions": [
                    {
                        "id" 2,
                        "groupId": 4,
                        "emoji": "👍",
                        "userId": "00000000-0000-0000-0000-000000000000",
                        "createdAt": "2022-01-01T00:00:00.000Z"
                    }
                ]
            },
            "pinnedMessagesIds": [6, 1, 3],
            "createdAt": "2022-01-01T00:00:00.000Z"
        },
        {
            "id": 7324,
            "ownerId": null,
            "tittle": null,
            "chatPhoto": null,
            "type": "Private",
            "memberIds": [
                "00000000-0000-0000-0000-000000000000"
                "00000000-0000-0000-0000-000000000000"
            ],
            "member": {
                "id": "00000000-0000-0000-0000-000000000000",
                "username": "admin",
                "emails": [
                    {
                        "data": "admin@gmail.com",
                        "isVerified": true,
                        "isPublic": true,
                        "addedAt": "2022-01-01T00:00:00.000Z"
                    }
                ],
                "usernameUpdateAd": "2022-01-01T00:00:00.000Z",
                "bio": "This is my bio",
                "profileImage": {
                    "url": "https://example.com/image.png",
                    "size": 1024,
                    "uploadedAt": "2022-01-01T00:00:00.000Z"
                },
                "globalName": "Admin",
                "createdAt": "2022-01-01T00:00:00.000Z",
            },
            "adminIds": [],
            "lastMessage": {
                "id" 43,
                "senderId": "00000000-0000-0000-0000-000000000000",
                "text": "Hey!",
                "sentAt": "2022-01-01T00:00:00.000Z",
                "updatedAt": "2022-01-01T00:00:00.000Z",
                "replyToId": null,
                "attachments": [
                    {
                        "type": "Video",
                        "url": "https://example.com/image.png",
                        "size": 1024,
                        "uploadedAt": "2022-01-01T00:00:00.000Z"
                    }
                ],
                "reactions": []
            },
            "pinnedMessagesIds": [6, 1, 3],
            "createdAt": "2022-01-01T00:00:00.000Z"
        }
    ]
}
```

```json
{
    "messages": [
        {
            "id" 43,
            "senderId": "00000000-0000-0000-0000-000000000000",
            "text": "Hey!",
            "sentAt": "2022-01-01T00:00:00.000Z",
            "updatedAt": "2022-01-01T00:00:00.000Z",
            "replyToId": null,
            "attachments": [
                {
                    "type": "Video",
                    "url": "https://example.com/image.png",
                    "size": 1024,
                    "uploadedAt": "2022-01-01T00:00:00.000Z"
                }
            ],
            "reactions": []
        },
        {
            "id" 44,
            "senderId": "00000000-0000-0000-0000-000000000000",
            "text": "What's up?",
            "sentAt": "2022-01-01T00:00:00.000Z",
            "updatedAt": null,
            "replyToId": null,
            "attachments": [],
            "reactions": []
        }
    ]
}
```

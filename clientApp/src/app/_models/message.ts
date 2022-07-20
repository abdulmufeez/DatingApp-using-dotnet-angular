export interface Message {
    id: number;
    senderId: number;
    senderName: string;
    senderUsername: string;
    senderPhotoUrl: string;
    recipientId: number;
    recipientName: string;
    recipientUsername: string;
    recipientPhotoUrl: string;
    content: string;
    messageSent: Date;
    messageRead: Date;
}
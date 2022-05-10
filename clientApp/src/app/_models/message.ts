export interface Message {
    id: number;
    senderId: number;
    senderName: string;
    senderPhotoUrl: string;
    recipientId: number;
    recipientName: string;
    recipientPhotoUrl: string;
    content: string;
    messageSent: Date;
    messageRead: Date;
}
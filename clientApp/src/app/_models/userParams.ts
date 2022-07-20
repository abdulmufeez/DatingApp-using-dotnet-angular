import { User } from "./User";

export class UserParams {
    gender: string;
    minAge = 18;
    maxAge: number;
    pageNumber = 1;
    pageSize = 24;
    orderBy = 'lastActive';

    constructor(user: User){
        this.gender = user.gender === 'female' ? 'male' : 'female';
        this.maxAge = user.age + 10;
    }
}
import { User } from "./User";

export class UserParams {
    gender: string;
    minAge = 18;
    maxAge: number;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';
    search: string;
    disableFiltering: string;

    constructor(user: User){
        this.gender = user.gender === 'female' ? 'male' : 'female';
        this.maxAge = user.age + 10;
        this.search = '';
        this.disableFiltering = 'false';        
    }
}
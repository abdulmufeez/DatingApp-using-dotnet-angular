export interface User { 
    id: number;
    username: string;
    photoUrl: string;
    token: string;
    gender: string;
    knownAs: string;
    age: number;
    roles: string[];
    isDisabled: boolean;
}
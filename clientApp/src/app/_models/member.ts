import { Photo } from "./photo";

export interface Member {
    id:                number;
    firstName:         string;
    lastName:          null;
    username:          string;
    mainPhotoUrl:      string;
    age:               number;
    knownAs:           string;
    profileCreatedAt:  Date;
    lastActive:        Date;
    gender:            string;
    introduction:      string;
    lookingFor:        string;
    interests:         string;
    country:           string;
    city:              string;
    isDisabled:        boolean;
    applicationUserId: number;
    photos:            Photo[];
}
import type {Booking} from './booking'

export interface StudySession extends Booking{

    course:string; 
    topic:string;
    description:string;
    isPublic:boolean;
    ParticipantCount:number; 
}
import type {Room} from './room'

export interface Booking{
    
    id? : number; 
    room:Room;
    startTime: string;
    endTime: string;
    isstudysession: Boolean; 
}
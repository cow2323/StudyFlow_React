import {Booking} from '../types/booking';
import {StudySession} from '../types/studysession';

const API_URL = import.meta.env.VITE_API_URL;

const headers = {
    'Content-Type':'application/json',
}; 

async function handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok){throw new Error(`Request failed (status ${response.status})`);
}
return response.json();
}

export async function getAllBookings(): Promise<Booking[]> {
    const response = await fetch(`${API_URL}/api/bookingapi/bookinglist`);
    return handleResponse<Booking[]>(response);}

export async function getAllStudySessions(): Promise<StudySession[]>{
    const response = await fetch(`${API_URL}/api/studysessionapi/sessionlist`);
    return handleResponse<StudySession[]>(response);}


export async function getBookingById(bookingId: number): Promise<Booking>{
        const response = await fetch(`${API_URL}/api/BookingAPI/getById?id=${bookingId}`)
        return handleResponse<Booking>(response); 
    }
export async function getStudySessionById(id: number): Promise<StudySession>{
        const response = await fetch(`${API_URL}StudySessionAPI/${id}`);
        return handleResponse<StudySession>(response); 
    }

export async function createBooking(booking: Booking): Promise<Booking> {
    const response = await fetch(`${API_URL}/api/BookingAPI/create`, {
        method: 'POST',
        headers,
        body: JSON.stringify(booking),
    });

    return handleResponse<Booking>(response);

}


export async function createStudySession(session: StudySession): Promise<StudySession> {
    const response = await fetch(`${API_URL}/api/StudySessionAPI/create`, {
        method: 'POST',
        headers,
        body: JSON.stringify(session),
    });

    return handleResponse<StudySession>(response);
}



export async function updateBooking(booking: Booking): Promise<Booking>{

    const response = await fetch(`${API_URL}/api/BookingAPI/update/${booking.id}`, 
        {method: 'PUT',
            headers, 
            body: JSON.stringify(booking),
        }
    );

    return handleResponse<Booking>(response); 
   
}


export async function updateSession(session: StudySession): Promise<StudySession>{
    const response = await fetch(`${API_URL}/api/StudySessionAPI/update/${session.id}`, 
        {method: 'PUT',
            headers, 
            body:JSON.stringify(session),
        }
    );

    return handleResponse<StudySession>(response); 
}


export async function deleteBooking(id:number): Promise<void>{

    //Works for both study sessions and Bookings
    const response = await fetch (`${API_URL}/api/BookingAPI/delete/${id}`,{
        method:'DELETE',
    });
    if (!response.ok) {
        throw new Error(`Failed to delete item (status)`)
    }
}

/* not needed


export async function deleteStudySession(id:number): Promise<void>{
    const response = await fetch (`${API_URL}/api/StudySessionAPI/delete/${id}`,{
        method:'DELETE',
    })
 if (!response.ok) {
        throw new Error(`Failed to delete item (status)`)
    }
}

*/ 

export async function joinStudySession(session: StudySession): Promise<StudySession>{
    const response = await fetch (`${API_URL}/api/StudySessionAPI/join/${session.id}`,
        {method:'PUT', 
       headers,
      body:JSON.stringify(session)})

      return handleResponse(response);
      
}



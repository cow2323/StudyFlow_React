const API_URL = "http://localhost:5245/api/rooms";

export async function getRooms() {
  const response = await fetch(API_URL);

  if (!response.ok) {
    throw new Error("Kunne ikke hente rommene.");
  }

  return response.json();
}
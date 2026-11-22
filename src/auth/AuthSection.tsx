import { Link } from  'react-router-dom'; 
import { useAuth } from './AuthContext' ;
import {NavLink} from "react-router-dom";


import {Nav, Dropdown } from 'react-bootstrap'; 


export default function AuthSection() {

    const {user, logout } = useAuth(); 


    return (
        <Nav> 
            {user ? (
                        <button className="nav-link text-white py-2"  onClick={logout}>
                        <i className="bi bi-person me-2" aria-hidden="true"></i>
                            Logout {user.sub}</button>
            ): (
                <>
                    <Nav.Link className="nav-link text-white py-2"  as={Link} to="/login">
                    <i className="bi bi-box-arrow-in-right me-2" aria-hidden="true"></i>
                    Login
                    </Nav.Link>
                </>
            )}
            </Nav>
    );
};
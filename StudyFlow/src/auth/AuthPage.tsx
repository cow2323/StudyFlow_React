import { useState } from "react";
import { Tabs, Tab, Row, Col } from "react-bootstrap";
import LoginForm from "./LoginForm";
import RegisterPage from "./RegisterPage";



export default function AuthPage() {

    //Authentication Page to handle login
    const [activeTab, setActiveTab] = useState<string>("login");

    function text (){ 
      return 

    };
    
    const welcomeText = text; 
    
   
    

  
    return (

      <Row className="g-0 shadow-lg rounded-4 overflow-hidden my-5">
        <Col lg={3} className="bg-primary bg-gradient text-white p-5 d-flex flex-column justify-content-center">
          <h2 className="fw-bold">🎓 StudyFlow</h2>
          <p className="lead">Your space to study together.</p>
        </Col>

        <Col lg={7} className="bg-white p-5">
        <h2 className="fw-bold mb-4">Welcome!</h2>
        <p className="fst-italic">{activeTab!="login"  ? 
      "Register new user": "Login to see rooms and administer bookings" } </p>

        <Tabs
          id="loginTabs"
          activeKey={activeTab}
          onSelect={(key) => setActiveTab(key ?? "Login")}
          className="mb-3"
          transition 
          defaultActiveKey="login"
        >
          <Tab eventKey="login" title="Login" >
            <LoginForm />
          </Tab>
          <Tab eventKey="register" title="Register">
            <RegisterPage onRegistered={() => setActiveTab("login")} />
          </Tab>
        </Tabs>
      </Col>
    </Row>

  );
}
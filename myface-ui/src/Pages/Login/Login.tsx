import React, {FormEvent, useContext, useState} from 'react';
import {Page} from "../Page/Page";
import {LoginContext} from "../../Components/LoginManager/LoginManager";
import "./Login.scss";
import {login} from "../../Api/apiClient";

export function Login(): JSX.Element {
    const loginContext = useContext(LoginContext);
    
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
  //const [failed login, set failed Login] = usestate(false)
    function tryLogin(event: FormEvent) {
        event.preventDefault();
          login(username, password).then (loginResponse =>{
              if(loginResponse== 200){loginContext.logIn()}})

        // do the encode username and password?
        //request to back end to log us in
        //bsed on the result user will be logger in or tell user to tryu again
        //var loginresponse = loging("dclhkcgvyhj.vh");
        loginContext.logIn();
    }
    
    return (
        <Page containerClassName="login">
            <h1 className="title">Log In</h1>
            <form className="login-form" onSubmit={tryLogin}>
                {/* //{failedlogin}? ,div> username or password incorrect</div> */}
                <label className="form-label">
                    Username
                    <input className="form-input" type={"text"} value={username} onChange={event => setUsername(event.target.value)}/>
                </label>

                <label className="form-label">
                    Password
                    <input className="form-input" type={"password"} value={password} onChange={event => setPassword(event.target.value)}/>
                </label>
                
                <button className="submit-button" type="submit">Log In</button>
            </form>
        </Page>
    );
}
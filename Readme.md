
To link the frontend (React) with the backend (Spring Boot), you need to configure the frontend to make HTTP requests to the backend endpoints for login and registration. Here’s how you can set this up:

### Step-by-Step Process

#### 1. Setup CORS in Spring Boot

To allow your React frontend to communicate with your Spring Boot backend, you need to configure CORS (Cross-Origin Resource Sharing).

Add the following CORS configuration in your Spring Boot application:

```java
package com.example.demo.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
public class WebConfig {

    @Bean
    public WebMvcConfigurer corsConfigurer() {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                registry.addMapping("/**")
                        .allowedOrigins("http://localhost:3000")
                        .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .allowedHeaders("*")
                        .allowCredentials(true);
            }
        };
    }
}
```

This configuration allows the React app (running on `http://localhost:3000`) to make requests to the Spring Boot backend.

#### 2. Set Up Axios in React

Install Axios in your React project for making HTTP requests:

```sh
npm install axios
```

#### 3. Create Service for API Calls

Create a new directory `src/services` and a file `AuthService.js` to handle API calls:

```javascript
// src/services/AuthService.js

import axios from 'axios';

const API_URL = 'http://localhost:8080/api/auth/';

class AuthService {
    login(username, password) {
        return axios.post(API_URL + 'login', { username, password });
    }

    register(username, email, password) {
        return axios.post(API_URL + 'register', { username, email, password });
    }
}

export default new AuthService();
```

#### 4. Implement Login and Registration Components

Here’s an example of how you can implement the login and registration components using React:

**Login Component:**

```javascript
// src/components/Login.js

import React, { useState } from 'react';
import AuthService from '../services/AuthService';

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            await AuthService.login(username, password);
            setMessage('Login successful!');
        } catch (error) {
            setMessage('Login failed!');
        }
    };

    return (
        <div>
            <h2>Login</h2>
            <form onSubmit={handleLogin}>
                <div>
                    <label>Username:</label>
                    <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
                </div>
                <div>
                    <label>Password:</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <button type="submit">Login</button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
}

export default Login;
```

**Register Component:**

```javascript
// src/components/Register.js

import React, { useState } from 'react';
import AuthService from '../services/AuthService';

function Register() {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState('');

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await AuthService.register(username, email, password);
            setMessage('Registration successful!');
        } catch (error) {
            setMessage('Registration failed!');
        }
    };

    return (
        <div>
            <h2>Register</h2>
            <form onSubmit={handleRegister}>
                <div>
                    <label>Username:</label>
                    <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} />
                </div>
                <div>
                    <label>Email:</label>
                    <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div>
                    <label>Password:</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <button type="submit">Register</button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
}

export default Register;
```

#### 5. Integrate Components in Your App

In your main `App.js` file, integrate the login and registration components:

```javascript
// src/App.js

import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';

function App() {
    return (
        <Router>
            <div>
                <Switch>
                    <Route exact path="/" component={Login} />
                    <Route path="/login" component={Login} />
                    <Route path="/register" component={Register} />
                </Switch>
            </div>
        </Router>
    );
}

export default App;
```

#### 6. Ensure React Router is Installed

If you haven't installed React Router, do so with:

```sh
npm install react-router-dom
```

### Summary

- **Backend (Spring Boot):** Ensure CORS is configured to allow requests from the frontend.
- **Frontend (React):** Use Axios for making HTTP requests and React Router for navigation.
- **Linking:** Use Axios in React components to make requests to the backend API endpoints.

Now, when you run both the backend and frontend, you should be able to register and login users, with the frontend making API calls to the backend.

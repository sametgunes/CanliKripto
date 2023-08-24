import React, { useState } from 'react';

const LoginPage = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleUsernameChange = (event) => {
        setUsername(event.target.value);
    };

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
    };

    const handleLogin = () => {
        // Kullanıcı adı ve şifre kontrolü
       

        // Başarılı giriş durumunda onLogin fonksiyonunu çağırarak ana sayfaya yönlendirin
        if (username === 'admin' && password === 'password') {
            onLogin();
        } else {
            alert('Hatalı kullanıcı adı veya şifre!');
        }
    };


    return (
        <div className="container">
            <h1 className="text-center mt-5">Please Login to See What We Have</h1>
            <div className="row justify-content-center mt-4">
                <div className="col-sm-6">
                    <div className="card">
                        <div className="card-body">
                            <form>
                                <div className="mb-3">
                                    <label htmlFor="username" className="form-label">Username:</label>
                                    <input
                                        type="text"
                                        className="form-control"
                                        id="username"
                                        value={username}
                                        onChange={handleUsernameChange}
                                    />
                                </div>
                                <div className="mb-3">
                                    <label htmlFor="password" className="form-label">Password:</label>
                                    <input
                                        type="password"
                                        className="form-control"
                                        id="password"
                                        value={password}
                                        onChange={handlePasswordChange}
                                    />
                                </div>
                                <div className="text-center">
                                    <button type="button" className="btn btn-primary" onClick={handleLogin}>Login</button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;

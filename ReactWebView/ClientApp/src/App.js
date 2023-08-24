import React, { useState } from 'react';
import LoginPage from './LoginPage';
import CryptoCurrencyTable from './CryptoCurrencyTable';

const App = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const handleLogin = () => {
        setIsLoggedIn(true);
    };

    return (
        <div>
            {isLoggedIn ? (
                <CryptoCurrencyTable />
            ) : (
                <LoginPage onLogin={handleLogin} />
            )}
        </div>
    );
};

export default App;

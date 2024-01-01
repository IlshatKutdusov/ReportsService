import React, { useContext } from 'react';
import { useHistory, useLocation } from 'react-router';
import { Link } from 'react-router-dom';
import { CurrentUserContext } from '../contexts/CurrentUserContext';
import logo from '../images/logo.png';

export default function Header({isLoggedIn, setIsLoggedIn}) {
  const location = useLocation();
  const currentUser = useContext(CurrentUserContext);
  const history = useHistory();
  
  function signOut() {
    localStorage.removeItem('jwt');
    setIsLoggedIn(false);
    history.push('/sign-in');
  }

  return (
    <header className="header">
      <img className="logo header__logo" src={logo} alt="Логотип с названием компании и иконкой отчёта"/>
      {
        location.pathname === '/signup' && 
          <p className="header__text">Уже зарегистрированы? <Link className="link" to='/signin'>Войти</Link></p>
      }
      
      {
        location.pathname === '/signin' && 
          <p className="header__text">Ещё не зарегистрированы? <Link className="link" to='/signup'>Регистрация</Link></p>
      }

      {
        isLoggedIn && 
          <div className="header__user-container">
            <span className="header__text">{currentUser.login}</span>
            <button className="header__button" onClick={signOut}>Выйти</button>
          </div>
      }
    </header>
  );
}
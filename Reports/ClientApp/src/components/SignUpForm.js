import React from 'react';

export default function SignUpForm({onSubmit}) {
  const [login, setLogin] = React.useState('');
  const [email, setEmail] = React.useState('');
  const [name, setName] = React.useState('');
  const [surname, setSurname] = React.useState('');
  const [password, setPassword] = React.useState('');

  function handleLoginInput(event) {
    setLogin(event.target.value);
  }

  function handleEmailInput(event) {
    setEmail(event.target.value);
  }

  function handleNameInput(event) {
    setName(event.target.value);
  }

  function handleSurnameInput(event) {
    setSurname(event.target.value);
  }

  function handlePasswordInput(event) {
    setPassword(event.target.value);
  }

  function handleSubmit(event) {
    event.preventDefault();

    onSubmit({
      login: login,
      email: email,
      name: name,
      surname: surname,
      password: password,
    });
  }

  return (
    <form className="form" action="#" onSubmit={handleSubmit}>
      <input className="form__input" type="email" placeholder="Email" required onChange={handleEmailInput} value={ email || '' } />
      <input className="form__input" type="text" placeholder="Имя" required onChange={handleNameInput} value={ name || '' } />
      <input className="form__input" type="text" placeholder="Фамилия" required onChange={handleSurnameInput} value={ surname || '' } />
      <input className="form__input" type="login" placeholder="Логин" required onChange={handleLoginInput} value={ login || '' } />
      <input className="form__input" type="password" placeholder="Пароль" required onChange={handlePasswordInput} value={ password || '' } />
      <button className="form__button form__button_type_registration" type="submit">Создать аккаунт</button>
    </form>
  )
}
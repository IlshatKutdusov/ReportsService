import React from 'react';
import { Route, Switch, useHistory } from 'react-router';
import Header from './Header';
import Footer from './Footer';
import Lobby from './Lobby';
import ProtectedRoute from './ProtectedRoute';
import SignInForm from './SignInForm';
import SignUpForm from './SignUpForm';
import Main from './Main';
import { CurrentUserContext } from '../contexts/CurrentUserContext';
import { api } from '../utils/Api';
import { setToken } from '../utils/token';

function App() {
  const [currentUser, setCurrentUser] = React.useState({});
  const [isLoggedIn, setIsLoggedIn] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);
  const [error, setError] = React.useState('');
  const [files, setFiles] = React.useState([]);
  const history = useHistory();

  function handleSignIn(authData) {
    setIsLoading(true);
    api
      .authenticate(authData)
      .then(response => {
        if (response.status === 'Success') {
          setToken(response.message.split(' ')[1]);
          api
            .getUserData(authData.login)
            .then((response) => {
              setCurrentUser(response.user);
              setIsLoggedIn(true);
              setFiles(response.user.files || []);
              history.push('/');
            })
            .catch(err => console.log(err));
        } else {
          setError(response.message);
        }        
      })
      .catch(error => console.log(error))
      .finally(() => setIsLoading(false));
  }

  function handleSignUp(regData) {
    setIsLoading(true);
    api
      .register(regData)
      .then(response => {
        
      })
      .catch(error => console.log(error))
      .finally(() => setIsLoading(false));
  }

  return (
    <div className="page">
      <CurrentUserContext.Provider value={currentUser}>
        <Header isLoggedIn={isLoggedIn} setIsLoggedIn={setIsLoggedIn} />

        <Switch>
            <Route exact path="/signin">
              <Lobby>
                <SignInForm onSubmit={handleSignIn} error={error} isLoading={isLoading}/>
              </Lobby>
            </Route>
            <Route exact path="/signup">
              <Lobby>
                <SignUpForm onSubmit={handleSignUp} />
              </Lobby>
            </Route>
          <ProtectedRoute component={Main} path="/" isLoggedIn={isLoggedIn} files={files} setFiles={setFiles} />
          </Switch>

        <Footer />
      </CurrentUserContext.Provider>
    </div>
  );
}

export default App;

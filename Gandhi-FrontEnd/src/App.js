import './App.css';
import { Routes, Route } from 'react-router-dom'
import Login from './Screens/Login';
import MainDashboard from './Screens/MainDashboard';
import ForgotPassword from './Screens/ForgotPassword';
import ManualRegistration from './Screens/ManualRegistration';
import AppNavbar from '../src/Comps/Internal Comps/Navbars/AppNavbar';
import ItemPage from './Screens/ItemPage';
import NewItemPage from './Screens/NewItemPage';
import DisconnectUserPage from './Screens/DisconnectUserPage';
import SearchPage from './Screens/SearchPage';
import PersonalArea from './Screens/PersonalArea';
import CategoriesSearchPage from './Screens/CategoriesSearchPage';
import Chat from './Screens/Chat';
import AuthProvider from './contexts/auth';
import DocumentTitle from 'react-document-title';

function App() {

  return (
    <DocumentTitle title='GANDHI'>
      <AuthProvider>
        <div className="App">
          <AppNavbar />
          <Routes>
            <Route exact path='/' element={<Login />} />
            <Route path='/MainDashboard' element={<MainDashboard />} />
            <Route path='/ForgotPassword' element={<ForgotPassword />} />
            <Route path='/Registration' element={<ManualRegistration />} />
            <Route path='/Item' element={<ItemPage />} />
            <Route path="/NewItemPage" element={<NewItemPage />} />
            <Route path="/Disconnect" element={<DisconnectUserPage />} />
            <Route path="/Search" element={<SearchPage />} />
            <Route path="/PersonalArea" element={<PersonalArea />} />
            <Route path="/Categories" element={<CategoriesSearchPage />} />
            <Route path="/Chat" element={<Chat />} />
          </Routes>
        </div>
      </AuthProvider>
    </DocumentTitle>
  );
}


export default App;

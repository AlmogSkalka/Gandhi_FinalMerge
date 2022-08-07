import React, { useEffect } from 'react'
import { useNavigate } from 'react-router-dom';
import { authentication, db } from '../Comps/External Connections/FireBase-config';
import { signOut } from 'firebase/auth';
import { updateDoc, doc } from 'firebase/firestore';
import Login from './Login';

export default function DisconnectUserPage() {

  const navigate = useNavigate();
  localStorage.removeItem('user')
  const handleSignout = async () => {
    await updateDoc(doc(db, 'users', authentication.currentUser.uid),
      {
        isOnline: false
      })
    await signOut(authentication)
    navigate('/')
  }

  useEffect(() => {
    handleSignout();
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])
  return (
    <Login />
  )
}

import { createContext, useEffect, useState } from "react";
import { onAuthStateChanged } from 'firebase/auth'
import { authentication } from "../Comps/External Connections/FireBase-config";
import { Spinner } from 'react-bootstrap'

export const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null)
    const [Loading, setLoading] = useState(true)
    useEffect(() => {
        onAuthStateChanged(authentication, (user) => {
            setUser(user)
            setLoading(false)
        })
    }, [])

    if (Loading) {
        return <div style={{ paddingTop: '30em' }}>
            <Spinner animation="border" />
        </div>
    }
    if (!Loading) {
        return (
            <AuthContext.Provider value={user}> {children} </AuthContext.Provider>
        )
    }
}

export default AuthProvider
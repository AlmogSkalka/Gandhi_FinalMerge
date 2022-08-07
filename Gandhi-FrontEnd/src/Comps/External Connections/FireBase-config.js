import { initializeApp } from 'firebase/app';
import { getAuth } from "firebase/auth";
import { getFirestore } from "firebase/firestore";
import { getStorage } from 'firebase/storage'
const firebaseConfig = {
    apiKey: "AIzaSyDVh8lTYtTF6_yUSCQRud-DYFzDSZvoEpM",
    authDomain: "gandhi-c0791.firebaseapp.com",
    databaseURL: "http://gandhi-c0791.firebaseio.com",
    projectId: "gandhi-c0791",
    storageBucket: "gandhi-c0791.appspot.com",
    messagingSenderId: "244500250054",
    appId: "1:244500250054:web:62da2efb15592cd30e86c7",
    measurementId: "G-HG64GK86R5"
};

const app = initializeApp(firebaseConfig);
const db = getFirestore(app)
const authentication = getAuth(app)
const storage = getStorage(app)
export { authentication, db, storage };


import React, { useEffect, useState } from 'react'
import {
    collection,
    query,
    where,
    onSnapshot,
    addDoc,
    Timestamp,
    orderBy,
    setDoc,
    doc,
    getDoc,
    updateDoc
} from 'firebase/firestore'
import { authentication, db, storage } from '../Comps/External Connections/FireBase-config'
import { ref, getDownloadURL, uploadBytes } from "firebase/storage";
import User from '../Comps/Internal Comps/Chat/User'
import MessageForm from '../Comps/Internal Comps/Chat/MessageForm'
import Message from '../Comps/Internal Comps/Chat/Message'
import { useLocation } from 'react-router-dom';

const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";

export default function Chat() {
    const [users, setUsers] = useState([])
    const [chat, setChat] = useState("")
    const [text, setText] = useState("")
    const [img, setImg] = useState('')
    const [Msgs, setMsgs] = useState([])
    const location = useLocation();

    const locationSellerId = { location }
    const locationSellerIdLocation = locationSellerId.location
    const LocationSellerIdState = locationSellerIdLocation.state
    const user1 = authentication.currentUser.uid

    useEffect(() => {
        const usersRef = collection(db, 'users')
        const q = query(usersRef, where('uid', 'not-in', [user1]))
        const unsub = onSnapshot(q, querySnapshot => {
            let users = [];
            querySnapshot.forEach(doc => {
                users.push(doc.data())
            });
            setUsers(users)
            const tmpSellers = []
            users.forEach(element => {
                fetch(ApiUrl + 'Users/GetUserSellItems?userId=' + element.GandhiUserId, {
                    method: 'GET',
                    headers: new Headers({
                        'Content-Type': 'application/json; charset=UTF-8',
                        'Accept': 'application/json; charset=UTF-8'
                    })
                })
                    .then(res => {
                        return res.json()
                    })
                    .then(
                        (result) => {
                            tmpSellers.push({ sellerId: element.GandhiUserId, items: result })
                        },
                        (error) => {
                        });
            }
            );
            try {
                if (LocationSellerIdState.SellerId) {
                    users.forEach(element => {
                        if (element.GandhiUserId !== undefined) {
                            if (element.GandhiUserId === LocationSellerIdState.SellerId) {
                                selectUser(element)
                            }
                        }
                    });
                }
            }
            catch {
            }
        })
        return () => unsub();
        //eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])
    //eslint-disable-next-line react-hooks/exhaustive-deps
    const selectUser = async (user) => {
        setChat(user)
        const user2 = user.uid
        const id = user1 > user2 ? `${user1 + user2}` : `${user2 + user1}`;
        const msgsRef = collection(db, 'messages', id, 'chat')
        const msgQuery = query(msgsRef, orderBy('createdAt', 'asc'))
        onSnapshot(msgQuery, querySnapshot => {
            let tmpMsgs = [];
            querySnapshot.forEach(doc => {
                tmpMsgs.push(doc.data())
            });
            setMsgs(tmpMsgs)
        })
        //checking if lastMsg exist between current user and clicked user
        const docSnap = await getDoc(doc(db, 'lastMsg', id))
        //if lstMsg exist and the msg is from the clicked user
        if (docSnap.data() && docSnap.data().from !== user1) {
            //update the lstMsg state in firebaseDB and turns the unread to false
            await updateDoc(doc(db, 'lastMsg', id), { unread: false })
        }
    }

    const handleSubmit = async e => {
        e.preventDefault();
        const user2 = chat.uid;
        const id = user1 > user2 ? `${user1 + user2}` : `${user2 + user1}`;
        let url;
        if (img) {
            const imgRef = ref(
                storage,
                `images/${new Date().getTime()} - ${img.name}`
            );
            //uploading the img to firebase DB
            const snap = await uploadBytes(imgRef, img)
            //getting the imgUrl from the firebaseDB
            const dlurl = await getDownloadURL(ref(storage, snap.ref.fullPath))
            url = dlurl;
        }
        await addDoc(collection(db, 'messages', id, 'chat'), {
            text,
            from: user1,
            to: user2,
            createdAt: Timestamp.fromDate(new Date()),
            //if url exist, then send it, if not... DONT
            media: url || ""
        });
        await setDoc(doc(db, 'lastMsg', id), {
            text,
            from: user1,
            to: user2,
            createdAt: Timestamp.fromDate(new Date()),
            media: url || "",
            unread: true
        })


    }

    return (
        <div className='chat_container'>
            <div className="users_container">
                {users.map((user) =>
                    <User
                        selectUser={selectUser}
                        key={user.uid}
                        user={user}
                        user1={user1}
                        chat={chat}
                    >
                    </User>
                )}
            </div>
            <div className="messages_container">
                {
                    chat ?
                        <>
                            <div className="messages_user">
                                <h3>{chat.name}</h3>
                            </div>
                            <div className="messages">
                                {Msgs.length ?
                                    Msgs.map((msg, i) => <Message key={i} user1={user1} msg={msg} />)
                                    :
                                    null
                                }
                            </div>
                            <MessageForm
                                handleSubmit={handleSubmit}
                                text={text}
                                setText={setText}
                                setImg={setImg}
                            />
                        </>
                        :
                        <h4 className="no_conv">
                            יש להקליק על משתמש בכדי להתחיל בשיחה
                        </h4>
                }
            </div>
        </div>
    )
}

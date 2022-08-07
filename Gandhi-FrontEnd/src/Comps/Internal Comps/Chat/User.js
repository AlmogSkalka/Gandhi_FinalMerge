import React, { useEffect, useState } from 'react'
import { onSnapshot, doc } from 'firebase/firestore';
import { db } from '../../External Connections/FireBase-config';

export default function User({ user, selectUser, user1, chat }) {
    const [data, setData] = useState('')
    const user2 = user.uid;

    useEffect(() => {
        const id = user1 > user2 ? `${user1 + user2}` : `${user2 + user1}`;
        let unsub = onSnapshot(doc(db, 'lastMsg', id), doc => {
            setData(doc.data())
        });
        return () => unsub()
        //eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    return (
        <>
            <div
                className={`user_wrapper ${chat.name === user.name && 'selected_user'}`}
                onClick={() => selectUser(user)}>
                <div className="user_info">
                    <div className="user_detail">
                        <img className='avatar' src={user.GandhiProfilePic ? user.GandhiProfilePic : "assets/img/gallery/gandhi logo.png"} alt={user} />
                        <h5>{user?.name}</h5>
                        {
                            data?.from !== user1 && data?.unread &&
                            <small className='unread'>הודעה חדשה</small>
                        }
                    </div>
                    <div
                        className={`user_status ${user.isOnline ? "online" : "offline"}`}
                    ></div>
                </div>
                {data && (
                    <p className="truncate">
                        <strong>{data.from === user1 ? "אני:" : null}</strong>
                        {data.text}
                    </p>
                )}
            </div >
            <div onClick={() => selectUser(user)}
                className={`sm_container ${chat.name === user.name && 'selected_user'}`}
            >
                <img className='avatar sm_screen' src={user.GandhiProfilePic ? user.GandhiProfilePic : "assets/img/gallery/gandhi logo.png"} alt={user} />

            </div>

        </>
    )
}

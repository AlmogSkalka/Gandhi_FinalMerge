import React, { useState, useEffect } from "react";
import * as IoIcons from "react-icons/io";
import * as BsIcons from "react-icons/bs";
import * as GrIcons from "react-icons/gr";
import { IconContext } from "react-icons";
import { Row, Form, Spinner } from "react-bootstrap";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { useNavigate } from "react-router";

const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";

export default function ItemComp(props) {
  const [ItemLikedByUser, setItemLikedByUser] = useState(false);
  const [InterestedUsers, setInterestedUsers] = useState([]);
  const [IsInterested, setIsInterested] = useState(false);
  const [IsSeller, setIsSeller] = useState(false);
  const [loading, setLoading] = useState(false);
  const [UserLocalStorage, setUserLocalStorage] = useState(
    JSON.parse(localStorage.getItem("user"))
  );

  const Item = props.Item;
  const nav = useNavigate();
  const imageWidth = '95%';
  useEffect(() => {
    if (UserLocalStorage.UserId === Item.SellerId) setIsSeller(true);
    try {
      if (
        UserLocalStorage.LikedItemsList !== undefined &&
        UserLocalStorage.LikedItemsList !== null
      ) {
        let tmpLSLikedItems = UserLocalStorage.LikedItemsList;
        for (let index = 0; index < tmpLSLikedItems.length; index++) {
          if (tmpLSLikedItems[index] === Item.ItemId) {
            setItemLikedByUser(true);
          }
        }
      }
    } catch {
      console.log("no liked items");
    }

    let garbageArr = [];
    garbageArr.push(Item.ItemId);
    if (UserLocalStorage.UserId !== Item.SellerId) {
      fetch(ApiUrl + "Users?userId=" + UserLocalStorage.UserId, {
        method: "POST",
        body: JSON.stringify(garbageArr),
        headers: new Headers({
          "Content-Type": "application/json; charset=UTF-8",
        }),
      })
        .then((res) => { })
        .then(
          (result) => { },
          (error) => {
            console.log("error when tried to get interested: ", error);
          }
        );
    }

    InterstedUsers();
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const InterstedUsers = () => {
    fetch(ApiUrl + "Items/GetInterestedUsersList?itemId=" + Item.ItemId, {
      method: "POST",
      body: "",
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
        Accept: "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          setInterestedUsers(result);
          result.forEach((u) => {
            if (u.UserId === UserLocalStorage.UserId) setIsInterested(true);
          });
        },
        (error) => {
          console.log(error);
        }
      );
  };

  const Redirect2Chat = () => {
    var sellerId = parseInt(Item.SellerId);
    var itemId = parseInt(Item.ItemId);
    nav("/Chat", { state: { SellerId: sellerId, ItemId: itemId } });
  };

  const Interested = () => {
    fetch(
      ApiUrl +
      "Users?userId=" +
      UserLocalStorage.UserId +
      "&itemId=" +
      Item.ItemId,
      {
        method: "POST",
        body: "",
        headers: new Headers({
          "Content-Type": "application/json; charset=UTF-8",
        }),
      }
    )
      .then((res) => { })
      .then(
        (result) => {
          Redirect2Chat();
        },
        (error) => {
          console.log("error when tried to get interested: ", error);
        }
      );
  };

  const makeTransaction = (character) => {
    let mtUrl =
      ApiUrl +
      "Items?itemId=" +
      Item.ItemId +
      "&sellerId=" +
      Item.SellerId +
      "&buyerId=" +
      UserLocalStorage.UserId +
      "&side=" +
      character;
    fetch(mtUrl, {
      method: "POST",
      body: "",
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
        Accept: "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })

      .then(
        (result) => {
          if (result === 1) alert("צריך להמתין שהקונה יאשר את הקנייה");
          else if (result === 2) alert("צריך להמתין שהמוכר יאשר את המכירה");
          else if (result === 3) alert("ברכותינו על מכירה");
          else if (result === 4) alert("ברכותינו על הפריט החדש");
          else alert("יש להמתין לצד השני שיאשר");

          nav("/PersonalArea");
        },
        (error) => {
          console.log(error);
        }
      );
  };

  const LikeItem = () => {
    setItemLikedByUser(true);

    fetch(
      ApiUrl +
      "Users?likedItemId=" +
      Item.ItemId +
      "&userId=" +
      UserLocalStorage.UserId +
      "&likeDirection=L",
      {
        method: "POST",
        body: "",
        headers: new Headers({
          "Content-Type": "application/json; charset=UTF-8",
          Accept: "application/json; charset=UTF-8",
        }),
      }
    )
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          //result should retrieve the new user object with the updated likelist as a field
          localStorage.setItem("user", JSON.stringify(result));
          setUserLocalStorage(result);
        },
        (error) => { }
      );
  };

  const UnlikeItem = () => {
    setItemLikedByUser(false);

    fetch(
      ApiUrl +
      "Users?likedItemId=" +
      Item.ItemId +
      "&userId=" +
      UserLocalStorage.UserId +
      "&likeDirection=U",
      {
        method: "POST",
        body: "",
        headers: new Headers({
          "Content-Type": "application/json; charset=UTF-8",
          Accept: "application/json; charset=UTF-8",
        }),
      }
    )
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          //result should retrieve the new user object with the updated likelist as a field
          localStorage.setItem("user", JSON.stringify(result));
          setUserLocalStorage(result);
        },
        (error) => { console.log("error while unliking the item: ", error) }
      );
  };

  const sellTransaction = (character, buyer) => {
    let mtUrl =
      ApiUrl +
      "Items?itemId=" +
      Item.ItemId +
      "&sellerId=" +
      Item.SellerId +
      "&buyerId=" +
      buyer +
      "&side=" +
      character;
    fetch(mtUrl, {
      method: "POST",
      body: "",
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
        Accept: "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })

      .then(
        (result) => {
          if (result === 1) alert("צריך להמתין שהקונה יאשר את הקנייה");
          else if (result === 2) alert("צריך להמתין שהמוכר יאשר את המכירה");
          else if (result === 3) alert("ברכותינו על מכירה");
          else if (result === 4) alert("ברכותינו על הפריט החדש");
          else alert("יש להמתין לצד השני שיאשר");
          nav("/PersonalArea");
        },
        (error) => {
          console.log(error);
        }
      );
  };

  const SellingItem = (event) => {
    event.preventDefault();
    sellTransaction("S", event.target.InterestedUserSelectList.value);
    setLoading(true);
  };

  if (Item && Item.ItemPhotos.length >= 1) {
    return (
      <div>
        <section className="py-0" id="outlet">
          <div className="container" id="Itemscontainer">
            <div className="row h-100 g-0" id="itemsRow">
              <div className="col-md-8">
                <div className="row h-100 g-0" id="itemsRow">
                  {Item?.ItemPhotos[2] ? (
                    <div className="col-md-4">
                      <img
                        className="img-item"
                        src={Item.ItemPhotos[2]}
                        width={imageWidth}
                        alt="..."
                      />
                    </div>
                  ) : null}
                  {Item.ItemPhotos[0] ? (
                    <div className="col-md-4">
                      <img
                        className="img-item"
                        src={Item.ItemPhotos[0]}
                        width={imageWidth}
                        alt=""
                      />
                    </div>
                  ) : null}
                  {Item.ItemPhotos[1] ? (
                    <div className="col-md-4">
                      <img
                        className="img-item"
                        src={Item.ItemPhotos[1]}
                        width={imageWidth}
                        alt="..."
                      />
                    </div>
                  ) : null}
                </div>
              </div>
              <div className="col-md-4 h-100">
                <div className="row h-100 g-0 itemsRow">
                  <h4> {Item.ItemDesc}</h4>
                  <div>
                    {IsSeller ? (
                      Item.ItemStatus === "N" ? (
                        <p>(הפריט הזה היה שייך לך בעבר)</p>
                      ) : (
                        <p>(הפריט הזה שייך לך)</p>
                      )
                    ) : null}
                  </div>
                  <div className="col-md-6 itemsRow">
                    <label>:מידה</label>
                    <p className="details">{Item.Size} </p>
                    <label>:סיבת מכירה</label>
                    <p className="details">{Item.SaleReason}</p>
                  </div>
                  <div className="col-md-6 h-100 itemsRow">
                    <label>:מותג</label>
                    <p className="details">{Item.Brand} </p>
                    <label>צבע:</label>
                    <p className="details"> {Item.Color}</p>
                  </div>
                  <div className="row h-100 g-0 itemsRow">
                    <label>:מחיר </label>
                    <h3 className="bold">{Item.Price}₪ </h3>
                  </div>
                  {Item.ItemStatus === "A" ? (
                    <div className="row h-100 g-0 itemsRow">
                      <div className="col-md-4 icons">
                        {IsSeller ? (
                          InterestedUsers.length ? (
                            <form onSubmit={SellingItem}>
                              <Row
                                style={{
                                  width: "100%",
                                  margin: "0px auto",
                                  padding: "0px auto",
                                  flexDirection: "row",
                                  alignItems: "center",
                                }}
                              >
                                <span>
                                  <Form.Select
                                    name="InterestedUserSelectList"
                                    className="select select-initialized"
                                  >
                                    {InterestedUsers.map((user, ind) => (
                                      <option key={ind} value={user.UserId}>
                                        {" "}
                                        {user.FullName}
                                      </option>
                                    ))}
                                  </Form.Select>
                                </span>
                                <span>
                                  <button
                                    disabled={loading}
                                    type="submit"
                                    className="btn btn-primary btn-block RoundElements"
                                  >
                                    {loading ? (
                                      <Spinner animation="border" />
                                    ) : (
                                      "למכור"
                                    )}
                                  </button>
                                </span>
                              </Row>
                            </form>
                          ) : (
                            <p style={{ direction: "rtl" }}>
                              לא התעניינו בפריט עד כה
                            </p>
                          )
                        ) : (
                          <>
                            <h5>אהבתי</h5>
                            <IconContext.Provider
                              value={{
                                color: "red",
                                size: "2em",
                                position: "fixed",
                                display: "absolute",
                              }}
                            >
                              {ItemLikedByUser ? (
                                <IoIcons.IoIosHeart onClick={UnlikeItem} />
                              ) : (
                                <IoIcons.IoIosHeartEmpty onClick={LikeItem} />
                              )}
                            </IconContext.Provider>
                          </>
                        )}
                      </div>
                      {IsSeller ? null : IsInterested ? ( //write here the components for the seller sid
                        <div className="col-md-6 icons">
                          <h5> קיבלתי את הפריט</h5>
                          <IconContext.Provider
                            value={{
                              color: "black",
                              size: "2em",
                              position: "fixed",
                              display: "absolute",
                            }}
                          >
                            <GrIcons.GrMoney
                              onClick={() => makeTransaction("B")}
                            />
                          </IconContext.Provider>
                        </div>
                      ) : (
                        <div className="col-md-6 icons">
                          <h5>אני מעוניין בפריט</h5>
                          <IconContext.Provider
                            value={{
                              color: "black",
                              size: "2em",
                              position: "fixed",
                              display: "absolute",
                            }}
                          >
                            <BsIcons.BsChatDots onClick={Interested} />
                          </IconContext.Provider>
                        </div>
                      )}
                    </div>
                  ) : null}
                </div>
              </div>
            </div>
          </div>
        </section>
      </div>
    );
  } else {
    alert("משהו קרה, לבינתיים נעביר אותכם לדף הבית");
    nav("/");
  }
}

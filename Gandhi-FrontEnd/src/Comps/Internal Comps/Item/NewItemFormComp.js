import React, { useState, useCallback, useEffect } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import "react-responsive-carousel/lib/styles/carousel.min.css"; // requires a loader
import { confirmAlert } from "react-confirm-alert";
import "react-confirm-alert/src/react-confirm-alert.css"; // Import css
import Creatable from "react-select/creatable";
import Slider from "@mui/material/Slider";
import Cropper from "react-easy-crop";
import { Spinner } from "react-bootstrap";
import getCroppedImg from "../../../cropImage";
import Modal from "react-modal";
import { useNavigate } from "react-router-dom";
import { Row, Col } from "react-bootstrap";

Modal.setAppElement("#root");
const axios = require("axios").default;
const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";
const ImagesUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar1/api/image";

export default function NewItemFormComp() {
  const [crop, setCrop] = useState({ x: 0, y: 0 });
  const [initialCroppedArea, setInitialCroppedArea] = useState(undefined);
  const [zoom, setZoom] = useState(1);
  const [ColorsList, setColorsList] = useState([]);
  const [BrandsList, setBrandsList] = useState([]);
  const [CategoriesList, setCategoriesList] = useState([]);
  const [SizesList, setSizesList] = useState([]);
  const [SaleReasonList, setSaleReasonList] = useState([]);
  const [TagsList, setTagsList] = useState([]);
  const [DepartmentsList, setDepartmentsList] = useState([]);
  const [UserDepartmentSelection, setUserDepartmentSelection] = useState([]);
  const [BoughtHereItems, setBoughtHereItems] = useState([]);
  const [ItemPhotosWithItemId, setItemPhotosWithItemId] = useState("");
  const [ItemId, setItemId] = useState("");
  const [ShowNewItem, setShowNewItem] = useState(true);
  const [ShowOldItem, setShowOldItem] = useState(false);
  const [DBTags, setDBTags] = useState([]);
  const [roleValue, setRoleValue] = useState("");
  const [Loading, setLoading] = useState(true);
  const [CroppingImg, setCroppingImg] = useState(null);
  const [showModal, setshowModal] = useState(false);
  const [croppedAreaPixels, setcroppedAreaPixels] = useState({});
  const [ShowCategories, setShowCategories] = useState(true);
  const [CroppedImages, setCroppedImages] = useState([]);
  const [ItemName, setItemName] = useState("");
  const [ItemLink, setItemLink] = useState("");
  const [ItemReason, setItemReason] = useState("");
  const [ItemPrice, setItemPrice] = useState(0);
  const [ItemColor, setItemColor] = useState("");
  const [ItemSize, setItemSize] = useState("");
  const [ItemBrand, setItemBrand] = useState("");
  const [ItemCategory, setItemCategory] = useState("");
  const [posted, setPosted] = useState(false);
  const navigate = useNavigate();
  const UserLocalStorage = JSON.parse(localStorage.getItem("user"));
  //general item functions
  useEffect(() => {
    fetch(ApiUrl + "Users?userId=" + UserLocalStorage.UserId, {
      method: "GET",
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
          setBoughtHereItems(result);
        },
        (error) => { }
      );
    GetTags();
    GetColorsList();
    GetBrandsList();
    GetSaleReasonList();
    GetSizesList();
    GetDepartmentsList();
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const submitItemFunction = (event) => {
    if (
      ItemName === "" ||
      ItemLink === "" ||
      ItemReason === "" ||
      ItemPrice === "" ||
      ItemColor === "" ||
      ItemSize === "" ||
      ItemBrand === "" ||
      ItemCategory === ""
    ) {
      alert("לא מילאת את כל השדות כמו שצריך!");
      event.preventDefault();
    } else if (
      ItemName !== "" &&
      ItemLink !== "" &&
      ItemReason !== "" &&
      ItemPrice !== "" &&
      ItemColor !== "" &&
      ItemSize !== "" &&
      ItemBrand !== "" &&
      ItemCategory !== ""
    ) {
      event.preventDefault();
      setPosted(true);
      const current = new Date();
      const TagsTextArr = [];
      for (let index = 0; index < TagsList.length; index++) {
        TagsTextArr.push(TagsList[index].label);
      }
      const item = {
        ItemDesc: ItemName,
        ItemOriginUrl: ItemLink,
        SaleReason: ItemReason,
        Price: ItemPrice,
        ColorId: ItemColor,
        SizeId: ItemSize,
        ItemStatus: "A",
        UploadDate: `${current.getFullYear()}/${current.getMonth() + 1
          }/${current.getDate()}`,
        BrandId: ItemBrand,
        ItemPhotos: "",
        ViewCounter: 0,
        CategoryId: ItemCategory,
        TagList: TagsTextArr,
      };
      PostNewItem(item);
    }
  };
  const PostNewItem = (item) => {
    fetch(ApiUrl + "Items?userId=" + UserLocalStorage.UserId, {
      method: "POST",
      body: JSON.stringify(item),
      headers: new Headers({
        "Content-Type": "application/json; charset=UTF-8",
      }),
    })
      .then((res) => {
        return res.json();
      })
      .then(
        (result) => {
          if (result !== 0) {
            setItemId(result);
            let tmpCroppedImg = [];
            CroppedImages.forEach((element) => {
              tmpCroppedImg.push(element);
            });
            setItemPhotosWithItemId(tmpCroppedImg);
          }
          if (result === 0) {
            alert("קרה משהו, אנא נסה שנית");
          }
        },
        (error) => {
          console.log(error);
        }
      );
  };
  const ShowNewItemForm = () => {
    setShowNewItem(true);
    setShowOldItem(false);
  };
  const SelectOldItemForm = () => {
    setShowOldItem(true);
    setShowNewItem(false);
  };
  const SellExistingItem = (itemObject) => {
    confirmAlert({
      title: "האם את.ה בטוח.ה בבחירה שלך?",
      message: "",
      buttons: [
        {
          label: "כן",
          onClick: () => {
            fetch(
              ApiUrl +
              "Items?itemId=" +
              itemObject.ItemId +
              "&userId=" +
              UserLocalStorage.UserId,
              {
                method: "PUT",
                body: "",
                headers: new Headers({
                  "Content-Type": "application/json; charset=UTF-8",
                }),
              }
            )
              .then((res) => {
                return res.json();
              })
              .then(
                (result) => {
                  if (result === 1) {
                    alert("הפריט עלה למכירה בהצלחה");
                  }
                  if (result === 0) {
                    alert("קרה משהו, אנא נסה שנית");
                  }
                },
                (error) => {
                  console.log(error);
                }
              );
          },
        },
        {
          label: "לא",
          onClick: () => alert("הפריט לא עלה למכירה כפי שביקשת."),
        },
      ],
    });
  };

  //Item images
  useEffect(() => {
    if (ItemPhotosWithItemId !== "" && ItemPhotosWithItemId.length >= 1) {
      const formdata = new FormData();
      let imgCounter = 1;
      try {
        CroppedImages.forEach((element) => {
          formdata.append(
            "myFile",
            element,
            "Item " + ItemId + " Photo " + imgCounter + ".jpeg"
          );
          imgCounter++;
        });
      } catch (error) {
        console.log(
          "Error occured when tried to upload item cropped photos: ",
          error
        );
      }
      axios
        .post(ImagesUrl, formdata)
        .then(function (response) {
          fetch(ApiUrl + "Items?ItemId=" + ItemId, {
            method: "POST",
            body: JSON.stringify(response.data),
            headers: new Headers({
              "Content-Type": "application/json; charset=UTF-8",
              Accept: "application/json; charset=UTF-8",
            }),
          }).then(
            (result) => {
              alert("הפריט נוסף בהצלחה!");
              navigate("/PersonalArea");
              setPosted(false);
            },
            (error) => { }
          );
        })
        .catch(function (error) {
          console.log(error);
        });
    }
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, [ItemPhotosWithItemId]);
  const handleOpenModal = () => {
    setshowModal(true);
  };
  const handleCloseModal = () => {
    setshowModal(false);
  };
  const onFileChange = async (event) => {
    let tmpEvent = event.target.files;
    for (let i = 0; i < tmpEvent.length; i++) {
      const file = tmpEvent[i];
      let imageDataUrl = await readFile(file);
      setCroppingImg(imageDataUrl);
      setCrop({ x: 0, y: 0 });
      setZoom(1);
      handleOpenModal();
    }
  };
  const readFile = (file) => {
    return new Promise((resolve) => {
      const reader = new FileReader();
      reader.addEventListener("load", () => resolve(reader.result), false);
      reader.readAsDataURL(file);
    });
  };
  useEffect(() => {
    const croppedArea = JSON.parse(window.localStorage.getItem("croppedArea"));
    setInitialCroppedArea(croppedArea);
  }, []);
  const showCroppedImage = async () => {
    try {
      const croppedImage = await getCroppedImg(
        CroppingImg,
        croppedAreaPixels,
        0
      );
      let tmpCroppedImgsArr = [];
      CroppedImages.forEach((element) => {
        tmpCroppedImgsArr.push(element);
      });
      tmpCroppedImgsArr.push(croppedImage);
      setCroppedImages(tmpCroppedImgsArr);
      setshowModal(false);
    } catch (e) {
      console.error(e);
    }
  };

  //eslint-disable-next-line react-hooks/exhaustive-deps
  const onCropComplete = useCallback((croppedArea, croppedAreaPixels) => {
    setcroppedAreaPixels(croppedAreaPixels);
  });

  //ItemTags funcs
  const customStyles = {
    option: (provided, state) => ({
      ...provided,
      borderBottom: "1px dotted pink",
      color: state.isSelected ? "red" : "blue",
      padding: "20",
    }),
  };
  const GetTags = () => {
    fetch(ApiUrl + "Tags/", {
      method: "GET",
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
          const tmpTags = result.map((tagObj) => {
            return {
              label: tagObj.TagDesc,
              value: String(tagObj.TagId),
            };
          });
          setDBTags(tmpTags);
          setLoading(true);
        },
        (error) => { }
      );
  };
  const handleChange = (field, value) => {
    switch (field) {
      case "roles":
        setRoleValue(value);
        break;
      default:
        break;
    }
    setTagsList(value);
  };

  //get db select fields
  const GetColorsList = () => {
    fetch(ApiUrl + "Colors/", {
      method: "GET",
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
          setColorsList(result);
        },
        (error) => { }
      );
  };
  const GetBrandsList = () => {
    fetch(ApiUrl + "Brands/", {
      method: "GET",
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
          setBrandsList(result);
        },
        (error) => { }
      );
  };
  const GetSaleReasonList = () => {
    fetch(ApiUrl + "Reasons/", {
      method: "GET",
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
          setSaleReasonList(result);
        },
        (error) => { }
      );
  };
  const GetSizesList = () => {
    fetch(ApiUrl + "Sizes/", {
      method: "GET",
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
          setSizesList(result);
        },
        (error) => { }
      );
  };
  const GetCategoriesList = () => {
    fetch(ApiUrl + "Categories/", {
      method: "GET",
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
          setCategoriesList(result);
          setShowCategories(false);
        },
        (error) => {
          console.log(error);
        }
      );
  };
  const GetDepartmentsList = () => {
    fetch(ApiUrl + "Departments/", {
      method: "GET",
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
          setDepartmentsList(result);
        },
        (error) => {
          console.log(error);
        }
      );
  };
  const setCategoryList = (event) => {
    setUserDepartmentSelection(event.target.value);
    GetCategoriesList();
  };

  //Form event.target.value into states
  const ItemNameChanged = (event) => {
    setItemName(event.target.value);
  };
  const ItemPriceChanged = (event) => {
    setItemPrice(event.target.value);
  };
  const ItemColorChanged = (event) => {
    setItemColor(event.target.value);
  };
  const ItemSizeChanged = (event) => {
    setItemSize(event.target.value);
  };
  const ItemSaleReasonChanged = (event) => {
    setItemReason(event.target.value);
  };
  const ItemBrandChanged = (event) => {
    setItemBrand(event.target.value);
  };
  const ItemLinkChanged = (event) => {
    setItemLink(event.target.value);
  };
  const ItemCategoryChanged = (event) => {
    setItemCategory(event.target.value);
  };
  //render
  if (!Loading) {
    return (
      <div style={{ paddingTop: "30em" }}>
        <Spinner animation="border" />
      </div>
    );
  } else if (Loading) {
    return (
      <div className="newItemForm">
        <h2>העלאת פריט למכירה</h2>
        <div className="container" style={{ marginBottom: "0.5em" }}>
          <div className="row h-100 g-0 itemformRow">
            <div className="col-md-6 ">
              <Button
                className="btn btn-block RoundElements ItemFormBtn"
                onClick={ShowNewItemForm}
              >
                פריט חדש
              </Button>
            </div>
            <div className="col-md-6 ">
              <Button
                className="btn btn-block RoundElements ItemFormBtn"
                onClick={SelectOldItemForm}
              >
                רכשתי פה
              </Button>
            </div>
          </div>
        </div>

        {ShowNewItem ? (
          <form className="form-outline mb-4 newItemForm">
            <div className="container newItemForm">
              <div className="row h-100 g-0 itemformRow">
                {/* item name */}
                <div className="form-outline mb-4">
                  <input
                    type="text"
                    className="form-control HebrewInputs"
                    name="itemName"
                    placeholder="שם הפריט"
                    onChange={ItemNameChanged}
                  />
                </div>
              </div>
              <div className="row h-100 g-0 itemformRow">
                {/* upload photo item */}
                <div className="form-outline mb-4">
                  <label htmlFor="#files">צירוף תמונה</label>
                  {CroppingImg ? (
                    <>
                      <br />
                      Choose Files לתמונה נוספת יש ללחוץ שוב על
                      <br />
                      <br />
                    </>
                  ) : (
                    ""
                  )}
                  <input
                    className="form-control HebrewInputs"
                    id="files"
                    type="file"
                    onChange={onFileChange}
                    multiple={false}
                    accept={"image/*"}
                  />
                </div>
                <div className="row h-100 g-0 itemformRow mb-4">
                  <div className="col-md-6 ">
                    <Form.Select
                      onChange={setCategoryList}
                      name="itemDepartment"
                      className="select select-initialized form-outline HebrewInputs bold"
                    >
                      <option
                        className="form-control "
                        value="null"
                        defaultValue={"none"}
                      >
                        גברים נשים או ילדים?
                      </option>
                      {DepartmentsList.map((Dep) => (
                        <option key={Dep.DepartmentId} value={Dep.DepartmentId}>
                          {Dep.DepartmentDesc}
                        </option>
                      ))}
                    </Form.Select>
                    <Form.Select
                      disabled={ShowCategories}
                      onChange={ItemCategoryChanged}
                      name="itemCategory"
                      className="select select-initialized form-outline HebrewInputs bold"
                    >
                      <option value="null" defaultValue="none">
                        קטגוריה
                      </option>
                      {CategoriesList.map((category, ind) =>
                        UserDepartmentSelection === category.DepartmentId ? (
                          <option key={ind} value={category.CategoryId}>
                            {category.CategoryDesc}
                          </option>
                        ) : null
                      )}
                    </Form.Select>
                  </div>
                  <div className="col-md-6">
                    <div className="select select-initialized form-outline HebrewInputs bold">
                      <input
                        type="number"
                        min="1"
                        className="form-control HebrewInputs"
                        name="itemPrice"
                        onChange={ItemPriceChanged}
                        placeholder="מחיר הפריט"
                      />
                    </div>
                    <Form.Select
                      name="itemColor"
                      onChange={ItemColorChanged}
                      className="select select-initialized form-outline HebrewInputs bold"
                    >
                      <option value="null" defaultValue={"none"}>
                        צבע
                      </option>
                      {ColorsList.map((color, ind) => (
                        <option key={ind} value={color.ColorId}>
                          {color.ColorDesc}
                        </option>
                      ))}
                    </Form.Select>
                  </div>
                </div>
                <div className="row h-100 g-0 itemformRow mb-4">
                  <div className="form-outline mb-4">
                    <label>תגיות</label>
                    <Creatable
                      isClearable
                      isMulti
                      onChange={(value) => handleChange("roles", value)}
                      options={DBTags}
                      value={roleValue}
                      styles={customStyles}
                    />
                  </div>

                  <div className="row h-100 g-0 itemformRow mb-4">
                    <div className="col-md-6 ">
                      <Form.Select
                        onChange={ItemSizeChanged}
                        name="itemSize"
                        className="select select-initialized form-outline HebrewInputs bold"
                      >
                        <option value="null" defaultValue={"none"}>
                          מידה
                        </option>
                        {SizesList.map((size, ind) => (
                          <option key={ind} value={size.SizeId}>
                            {size.SizeDesc}
                          </option>
                        ))}
                      </Form.Select>
                      <div className="form-outline mb-4">
                        <input
                          type="text"
                          className="form-control HebrewInputs"
                          name="itemLink"
                          onChange={ItemLinkChanged}
                          placeholder="צירוף קישור"
                        />
                      </div>
                    </div>
                    <div className="col-md-6 ">
                      <Form.Select
                        name="itemSaleReason"
                        onChange={ItemSaleReasonChanged}
                        className="select select-initialized form-outline HebrewInputs"
                      >
                        <option value="null" defaultValue={"none"}>
                          סיבת מכירה
                        </option>
                        {SaleReasonList.map((reason, ind) => (
                          <option key={ind} value={reason.ReasonId}>
                            {reason.ReasonDesc}
                          </option>
                        ))}
                      </Form.Select>
                      <Form.Select
                        name="itemBrand"
                        onChange={ItemBrandChanged}
                        className="select select-initialized form-outline HebrewInputs"
                      >
                        <option value="null" defaultValue={"none"}>
                          מותג
                        </option>
                        {BrandsList.map((brand, ind) => (
                          <option key={ind} value={brand.BrandId}>
                            {brand.BrandDesc}
                          </option>
                        ))}
                      </Form.Select>
                    </div>
                  </div>
                  <button
                    type="submit"
                    disabled={posted}
                    onClick={submitItemFunction}
                    id="ItemBtn"
                    className="btn btn-block RoundElements"
                  >
                    {" "}
                    {!posted ? (
                      "העלה פריט למכירה"
                    ) : (
                      <Spinner animation="border" />
                    )}
                  </button>
                </div>
              </div>
            </div>
          </form>
        ) : null}
        {ShowOldItem ? (
          <div>
            <>
              <Row style={{ margin: "0px auto", width: "50em" }}>
                <Col>
                  <h4> פריטים שקנית פה</h4>
                </Col>
              </Row>
              <Row>
                {BoughtHereItems.map((item, ind) => (
                  <Col>
                    <div
                      className="search-card"
                      key={ind}
                      style={{ padding: "1em" }}
                    >
                      <div>
                        <img
                          className="searchCards"
                          src={item.ItemPhotos[0]}
                          alt="..."
                          style={{
                            width: "200px",
                            margin: "0px auto",
                            borderRadius: "0.5rem",
                          }}
                          onClick={() => SellExistingItem(item)}
                        />
                        <div>
                          <div>{item.ItemDesc}</div>
                        </div>
                      </div>
                    </div>
                  </Col>
                ))}
              </Row>
            </>
          </div>
        ) : null}

        <Modal
          isOpen={showModal}
          contentLabel="Minimal Modal Example"
          style={{ height: "35em", width: "35em", paddingTop: "50em" }}
        >
          <Button
            variant="primary"
            id="CropImageModalBTN"
            onClick={handleCloseModal}
          >
            זו לא התמונה הנכונה
          </Button>
          {CroppingImg ? (
            <>
              <div className="crop-container">
                <Cropper
                  image={CroppingImg}
                  crop={crop}
                  zoom={zoom}
                  aspect={3 / 4}
                  onCropChange={setCrop}
                  onCropComplete={onCropComplete}
                  onZoomChange={setZoom}
                  initialCroppedAreaPercentages={initialCroppedArea}
                />
              </div>
              <div className="controls">
                <Slider
                  value={zoom}
                  min={1}
                  max={3}
                  step={0.1}
                  aria-labelledby="Zoom"
                  onChange={(e, zoom) => setZoom(zoom)}
                  classes={{ container: "slider" }}
                />
              </div>
              <Button
                onClick={showCroppedImage}
                color="primary"
                id="CropImageModalBTN"
              >
                גזירת תמונה
              </Button>
            </>
          ) : null}
        </Modal>
      </div>
    );
  }
}

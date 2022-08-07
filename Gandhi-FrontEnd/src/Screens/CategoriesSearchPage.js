import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router'
import Search from '../Comps/Internal Comps/Search/Search';
import { useLocation } from 'react-router-dom';
import Modal from 'react-modal/lib/components/Modal';
import * as GiIcons from "react-icons/gi";

const customStyles = {
    content: {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        marginRight: '-50%',
        transform: 'translate(-50%, -50%)',
        zIndex: 99,
        width: '85%',
        height: '75%',
    },
};

const ApiUrl = "https://proj.ruppin.ac.il/igroup64/test2/tar2/api/";
export default function CategoriesSearchPage() {
    const [Top4Categories, setTop4Categories] = useState([])
    const [TopCategory, setTopCategory] = useState([])
    const [WideImg, setWideImg] = useState(true)
    const [ShowModal, setShowModal] = useState(false);
    const [CategoryItems, setCategoryItems] = useState([])
    const { state } = useLocation();
    const Items = { state }.state;
    const navigate = useNavigate();

    useEffect(() => {
        fetch(ApiUrl + 'Departments/GetTopCategories?departmentId=' + { state }.state[0].DepartmentID, {
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
                    let tmpResult = [];
                    for (let i = 1; i < result.length; i++) {
                        tmpResult.push(result[i])
                    }
                    setTop4Categories(tmpResult)
                    setTopCategory(result[0])
                },
                (error) => {
                    console.log(error)
                });
        //eslint-disable-next-line react-hooks/exhaustive-deps
    }, [Items])

    const ShowCategoryItems = (categoryId) => {
        let tmpItems = [];
        Items.forEach(element => {
            if (element.CategoryId === categoryId)
                tmpItems.push(element)
        });
        setCategoryItems(tmpItems)
        handleOpenModal(true)
    }

    const handleOpenModal = () => {
        window.scroll({
            top: document.body.offsetHeight,
            left: 0,
            behavior: 'smooth',
        });

        setShowModal(true)
    }

    const Redirect2ItemPage = (itemObject) => {
        navigate("/Item", { state: itemObject });
    }

    return (
        <main style={{ paddingTop: '7em' }} className="main" id="top">
            <Modal
                id='CategoriesItemsModal'
                isOpen={ShowModal}
                onRequestClose={() => setShowModal(false)}
                style={customStyles}
                contentLabel="Example Modal"
            >
                {
                    <section className="py-0">
                        <div className="container">
                            <div className="row h-100">
                                <div className="col-lg-7 mx-auto text-center mt-7 mb-5">
                                    <h5 className="fw-bold fs-3 fs-lg-5 lh-sm">פריטי המחלקה</h5>
                                    <p>יש לגלול מטה על מנת לצפות בפריטים</p>

                                </div>
                                <div className="col-12">
                                    <div className="carousel slide" id="carouselBestDeals" data-bs-touch="false" data-bs-interval="false">
                                        <div className="carousel-inner">
                                            <div className="carousel-item active" data-bs-interval="10000">
                                                <div className="CategoryCarousel row h-100 align-items-center g-2">
                                                    {
                                                        CategoryItems?.length >= 1 ?
                                                            CategoryItems.map((Item, ind) =>
                                                                <div id='categoriesDiv' key={ind} className="col-sm-6 col-md-3 mb-3 mb-md-0 h-100" >
                                                                    <div onClick={() => Redirect2ItemPage(Item)} className="card card-span h-100 text-white"><img className="img-fluid h-100" src={Item.ItemPhotos[0]} alt="..." />
                                                                        <div className="card-img-overlay ps-0"> </div>
                                                                        <div className="CategoryCarousel card-body ps-0 bg-200">
                                                                            <h5 className="fw-bold text-1000 text-truncate">{Item.ItemDesc}</h5>
                                                                            <h5 className="CategoryCarouselh5 fw-bold text-1000 text-truncate">
                                                                                {Item.Price} <GiIcons.GiTwoCoins /> :מחיר
                                                                            </h5>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            )
                                                            :
                                                            <p>אין פריטים עבור מחלקה זו לצערנו הרב</p>
                                                    }
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                }

            </Modal>
            <section className="py-0" id="outlet">
                <div className="container">
                    <div className="row h-100 g-0">
                        <div className="col-md-6">
                            <div className="card card-span h-100 text-white"><img className="card-img h-100" src={TopCategory.CategoryPicUrl} alt="..." />
                                <div className="card-img-overlay bg-dark-gradient rounded-0">
                                    <div className="p-5 p-md-2 p-xl-5 d-flex flex-column flex-end-center align-items-baseline h-100">
                                        <h1 className="fs-md-4 fs-lg-7 text-light">{TopCategory.CategoryDesc} </h1>
                                    </div>
                                </div>
                                <p className="stretched-link" onClick={() => ShowCategoryItems(TopCategory.CategoryId)}></p>
                            </div>
                        </div>
                        <div className="col-md-6 h-100">
                            <div className="row h-100 g-0">
                                {
                                    Top4Categories.map((Category, ind) =>
                                        WideImg ?
                                            <div key={ind} onClick={() => ShowCategoryItems(Category.CategoryId)} className="col-md-6 h-100">
                                                <div className="card card-span h-100 text-white"><img className="card-img h-100" src={Category.CategoryPicUrl} alt="..." />
                                                    <div className="card-img-overlay bg-dark-gradient rounded-0">
                                                        <div className="p-5 p-xl-5 p-md-0">
                                                            <h3 className="text-light">{Category.CategoryDesc} </h3>
                                                        </div>
                                                    </div>
                                                    <p className="stretched-link" onClick={() => ShowCategoryItems(Category.CategoryId)}></p>
                                                </div>
                                                {setWideImg(false)}
                                            </div>
                                            :
                                            <div key={ind} onClick={() => ShowCategoryItems(Category.CategoryId)} className="col-md-6">
                                                <div className="card card-span h-100 text-white"><img className="card-img h-100" src={Category.CategoryPicUrl} alt="..." />
                                                    <div className="card-img-overlay bg-dark-gradient rounded-0">
                                                        <div className="p-5 p-xl-5 p-md-0">
                                                            <h3 className="text-light">{Category.CategoryDesc}</h3>
                                                        </div>
                                                    </div>
                                                    <p className="stretched-link" onClick={() => ShowCategoryItems(Category.CategoryId)}></p>
                                                </div>
                                            </div>
                                    )
                                }

                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <div style={{ margin: '0px auto' }}> <Search details={state} /></div>
        </main >
    )
}

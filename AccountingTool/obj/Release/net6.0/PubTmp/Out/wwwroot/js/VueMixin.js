const VueMxin = {
    data() {
        return {
            token: null
        }
    }
    , methods: {
        QueryPageLoadingStart() {
            const loadingPage = document.getElementById("query-loading-page");
            loadingPage.classList.remove("d-none");
        },
        QueryPageLoadingEnd() {
            const loadingPage = document.getElementById("query-loading-page");
            loadingPage.classList.add("d-none");
        },
        loadingStart() {
            const loadingPage = document.getElementById("loading-page");
            loadingPage.classList.remove("d-none");
        },
        loadingEnd() {
            const loadingPage = document.getElementById("loading-page");
            loadingPage.classList.add("d-none");
        },
        storeSearchDataCookie() {
            document.cookie = `category=${this.category}; path=/`;
            document.cookie = `startDate=${this.dateTransfer(this.searchDate.startDate)}; path=/`;
            document.cookie = `endDate=${this.dateTransfer(this.searchDate.endDate)}; path=/`;
        },
        setSearchData() {
            this.category = this.getCookie("category");
            this.searchDate.startDate = this.getCookie("startDate");
            this.searchDate.endDate = this.getCookie("endDate");
        },
        clearSearchDataCookie() {
            this.deleteCookie("category");
            this.deleteCookie("startDate");
            this.deleteCookie("endDate");
        },
        getToken() {
            return this.getCookie("accountingToolToken")
        },
        getCookie(cookieName) {
            const value = `; ${document.cookie}`;
            const parts = value.split(`; ${cookieName}=`);
            if (parts.length === 2) {
                return parts.pop().split(';').shift();
            }
        },
        deleteCookie(cookieName) {
            const date = new Date();
            date.setTime(date.getTime() - 1000000);
            document.cookie = `${cookieName}='';expires=` + date.toGMTString();
        },
        dateTransfer(dateRwData) {
            const formDate = new Date(dateRwData);
            const year = formDate.getFullYear();
            const month = String(formDate.getMonth() + 1).padStart(2, "0");
            const date = String(formDate.getDate()).padStart(2, "0")
            const dateTransfered = `${year}-${month}-${date}`;
            return dateTransfered;
        },
        dateTransferChWords(dateRwData) {
            const formDate = new Date(dateRwData);
            const year = formDate.getFullYear();
            const month = String(formDate.getMonth() + 1).padStart(2, "0");
            const date = String(formDate.getDate()).padStart(2, "0")
            const dateTransfered = `${year}年${month}月${date}日`;
            return dateTransfered;
        },
        dateTransferToMonthChWords(dateRwData) {
            const formDate = new Date(dateRwData);
            const year = formDate.getFullYear();
            const month = String(formDate.getMonth() + 1).padStart(2, "0");
            const dateTransfered = `${year}年${month}月`;
            return dateTransfered;
        },
        // 開啟modal
        modalShow(modalTitle, modalSubTitle, resultType) {
            const modalBody = document.getElementById("modal-body");
            modalMessage = "";
            //寫入icon
            if (resultType === "succcess") {
                modalMessage = '<i class="bi bi-check-circle me-3 fs-2"></i>';
            } else if (resultType === "error") {
                modalMessage = '<i class="bi bi-x-circle me-3 fs-3"></i>';
            }
            //寫入標題
            if (modalTitle) {
                modalMessage += `<h3 class="m-3">${modalTitle}</h3>`;
            }
            //寫入子標題
            if (modalSubTitle) {
                if (Array.isArray(modalSubTitle)) {
                    subTitileMessageHtmlTags = modalSubTitle.reduce((accumulator, currentValue) => {
                        accumulator = accumulator + `<p class="mt-0 fs-5">${currentValue}</p>`;
                        return accumulator;
                    }, "")

                    modalMessage += subTitileMessageHtmlTags
                } else {
                    modalMessage += `<p class="mt-0 fs-5">${modalSubTitle}</p>`
                }
            }
            modalBody.innerHTML = modalMessage;
            
            const myModal = new bootstrap.Modal(document.getElementById('myModal'), {
                keyboard: false
            })
            myModal.show();
        },
        getAxiosData(data) {
            const axiosData = {}
            for (let key in data) {
                axiosData[key] = data[key].value
            }
            return axiosData
        },
        validate(data) {
            const validationErrorMessage = [];
            for (let key in data) {
                const currentItem = data[key];
                //必填項目驗證
                if (currentItem.validateTypes.indexOf("required") != -1) {
                    if (!currentItem.value) {
                        validationErrorMessage.push(`${currentItem.columnName}為必填項目`);
                    }
                }

                //信箱格式驗證
                if (currentItem.validateTypes.indexOf("email") != -1) {
                    const rule = new RegExp(/[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/)
                    const emailValidationResult = rule.test(currentItem.value);
                    if (!emailValidationResult) {
                        validationErrorMessage.push(`${currentItem.columnName}格式不符`);
                    }
                }
            }
            return validationErrorMessage;
        }
    }
};
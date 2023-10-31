const VueMxin = {
    data() {
        return {
            token: null
        }
    }
    , methods: {
        loadingStart() {
            const loadingPage = document.getElementById("loading-page");
            loadingPage.classList.remove("d-none");
        },
        loadingEnd() {
            const loadingPage = document.getElementById("loading-page");
            loadingPage.classList.add("d-none");
        },
        getToken() {
            const value = `; ${document.cookie}`;
            const parts = value.split("; accountingToolToken=");
            if (parts.length === 2) {
                return parts.pop().split(';').shift();
            }
        },
        dateTransfer(dateRwData) {
            const formDate = new Date(dateRwData);
            const year = formDate.getFullYear();
            const month = String(formDate.getMonth() + 1).padStart(2, "0");
            const date = String(formDate.getDate()).padStart(2, "0")
            const dateTransfered = `${year}-${month}-${date}`;
            return dateTransfered;
        },
        dateTransferToMonth(dateRwData) {
            const formDate = new Date(dateRwData);
            const year = formDate.getFullYear();
            const month = String(formDate.getMonth() + 1).padStart(2, "0");
            const dateTransfered = `${year}-${month}`;
            return dateTransfered;
        },
        // 開啟modal
        modalShow(modalMessage, resultType) {
            const modalBody = document.getElementById("modal-body");

            if (Array.isArray(modalMessage)) {
                modalMessage = modalMessage.reduce((accumulator, currentValue) => {
                    accumulator = accumulator + `<p>${currentValue}</p>`;
                    return accumulator;
                }, "")
            }
            
            if (resultType === "succcess") {
                modalMessage = '<i class="bi bi-check-circle me-3"></i>' + modalMessage;
            } else if (resultType === "error") {
                modalMessage = '<i class="bi bi-x-circle me-3"></i>' + modalMessage;
            }

            modalBody.innerHTML = modalMessage;
            
            var myModal = new bootstrap.Modal(document.getElementById('myModal'), {
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
                    const rule = new RegExp("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")
                    const emailValidationResult = rule.test();
                    if (!emailValidationResult) {
                        validationErrorMessage.push(`${currentItem.columnName}格式不符`);
                    }
                }
                
            }
            return validationErrorMessage;
        }
    }
};
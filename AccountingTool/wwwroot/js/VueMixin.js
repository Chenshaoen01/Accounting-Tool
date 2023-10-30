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
        }
    }
};
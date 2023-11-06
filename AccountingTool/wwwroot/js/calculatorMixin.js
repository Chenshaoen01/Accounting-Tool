const calculatorMixin = {
    data() {
        return {
            calculator: {
                processDescription: "",
                numberList: [
                    { type: "string", value: "1", buttonContent: "1" },
                    { type: "string", value: "2", buttonContent: "2" },
                    { type: "string", value: "3", buttonContent: "3" },
                    { type: "string", value: "4", buttonContent: "4" },
                    { type: "string", value: "5", buttonContent: "5" },
                    { type: "string", value: "6", buttonContent: "6" },
                    { type: "string", value: "7", buttonContent: "7" },
                    { type: "string", value: "8", buttonContent: "8" },
                    { type: "string", value: "9", buttonContent: "9" },
                    { type: "string", value: "00", buttonContent: "00" },
                    { type: "string", value: "0", buttonContent: "0" },
                    { type: "string", value: ".", buttonContent: "." },
                ],
                buttonList: [
                    { type: "icon", value: "+", buttonContent: "bi bi-plus" },
                    { type: "string", value: "clear", buttonContent: "AC" },
                    { type: "icon", value: "-", buttonContent: "bi bi-dash" },
                    { type: "icon", value: "backSpace", buttonContent: "bi bi-arrow-left" },
                    { type: "icon", value: "*", buttonContent: "bi bi-x" },
                    { type: "string", value: "=", buttonContent: "=" },
                    { type: "string", value: "/", buttonContent: "÷" },
                    { type: "icon", value: "send", buttonContent: "bi bi-send" },
                ]
            }
        }
    }
    , methods: {
        calculatorButtonFunction(buttonValue) {
            const specilButtonValue = ["clear", "backSpace", "send"]
            // 判斷是否為特殊按鈕
            const specialButton = ["clear", "backSpace", "=", "send"];
            const notSpecialButton = specialButton.indexOf(buttonValue) == -1;

            const calculatorSign = ["+", "-", "*", "/", "="];
            const lastString = this.calculator.processDescription.substr(this.calculator.processDescription.length - 1, 1)
            const islastStringCaculator = calculatorSign.indexOf(lastString) != -1;
            const isCurrentButtonCalculator = calculatorSign.indexOf(buttonValue) != -1;

            if (notSpecialButton) {
                if (isCurrentButtonCalculator) {
                    if (islastStringCaculator) {
                        // 如果按鈕為運算子且算式最後的字元是為運算子則替換最後一個字元
                        this.calculator.processDescription = this.calculator.processDescription.substring(0, this.calculator.processDescription.length - 1)
                        this.calculator.processDescription = this.calculator.processDescription + buttonValue
                    } else {
                        // 如果按鈕為運算子且算式最後的字元是為運算子則替換最後一個字元
                        this.calculator.processDescription = this.calculator.processDescription + buttonValue
                    }
                } else {
                    // 如果按鈕為數字則寫入算式
                    this.calculator.processDescription = this.calculator.processDescription + buttonValue
                }
            } else {
                if (buttonValue == "clear") {
                    // 清空按鈕
                    this.calculator.processDescription = ""
                } else if (buttonValue == "backSpace") {
                    debugger
                    // backSpace按鈕
                    this.calculator.processDescription = this.calculator.processDescription.substring(0, this.calculator.processDescription.length - 1)
                    debugger
                } else if (buttonValue == "=") {
                    // 計算按鈕
                    const scriptStringArr = this.calculator.processDescription.split("")
                    scriptStringArr.push("=")
                    // 當前運算數字
                    let currentNumber = ""
                    // 當前運算子
                    let currentCaculator = ""
                    const calculateResult = scriptStringArr.reduce((accumulator, currentValue) => {
                        const isNumber = calculatorSign.indexOf(currentValue) == -1;
                        if (isNumber) {
                            currentNumber = currentNumber + currentValue
                            return accumulator
                        } else {
                            if (accumulator) {
                                // 如果目前計算結果不是空值，填入目前計算結果、運算子、本次運算數字進行計算
                                accumulator = this.calculate(accumulator, currentCaculator, currentNumber)
                            } else {
                                // 如果目前計算結果是空的，以currentValue作為結果(第一個運算子)
                                accumulator = currentNumber
                            }
                            currentCaculator = currentValue
                            currentNumber = ""
                            return accumulator
                        }
                    }, "")
                    // 更新畫面上的計算結果
                    this.calculator.processDescription = calculateResult.toString();
                } else if (buttonValue == "send") {
                    // 送出按鈕
                    this.form.price.value = this.calculator.processDescription
                }
            }
        }
        , calculate(valueBeforeCount, calculator, calculateValue) {
            if (calculator == "+") {
                return parseFloat(valueBeforeCount) + parseFloat(calculateValue)
            } else if (calculator == "-") {
                return parseFloat(valueBeforeCount) - parseFloat(calculateValue)
            } else if (calculator == "*") {
                return parseFloat(valueBeforeCount) * parseFloat(calculateValue)
            } else if (calculator == "/") {
                return parseFloat(valueBeforeCount) / parseFloat(calculateValue)
            }
        }
    }
};
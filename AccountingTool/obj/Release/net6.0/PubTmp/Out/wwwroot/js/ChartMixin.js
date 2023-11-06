const ChartMxin = {
    data() {
        return {
            chart: null,
            colorList: ['Violet', 'Khaki', 'PeachPuff', 'DarkGrey', 'SpringGreen', 'LightSteelBlue', 'NavajoWhite', 'HotPink', 'DarkKhaki', 'PaleGoldenrod', 'RebeccaPurple', 'DimGray', 'SkyBlue', 'MediumSeaGreen', 'RosyBrown', 'PaleVioletRed', 'Plum', 'Salmon', 'DodgerBlue', 'Wheat'],
            doughnutChartContent: {
                type: 'doughnut',
                data: {
                    labels: [],
                    datasets: [
                        {
                            label: '',
                            data: [],
                            backgroundColor: [],
                        }
                    ],
                    totalPrice: null
                },
                options: {
                    animation: {
                        duration: 0
                    },
                    responsive: true,
                    plugins: {
                        datalabels: {
                            anchor: 'center',
                            align: 'center',
                            formatter: function (value, context) {
                                console.log(value/context.chart.data.totalPrice)
                                return context.chart.data.labels[context.dataIndex] + ":" + value + "元(" + Math.round(value/context.chart.data.totalPrice*100) + '%' + ")";
                            },
                            font: {
                                weight: 'bold',
                                size: 12
                            }
                        },
                        legend: {
                            position: 'bottom',
                        },
                    },
                },
            },
            lineChartContent: {
                type: 'line',
                data: {
                    labels: [],
                    datasets: [
                    {
                        label: '每月盈餘',
                        data: [],
                        fill: false,
                        borderColor: 'Violet',
                        tension: 0.1
                        },
                    {
                        label: '每月支出',
                        data: [],
                        fill: false,
                        borderColor: 'Khaki',
                        tension: 0.1
                        },
                    {
                        label: '每月收入',
                        data: [],
                        fill: false,
                        borderColor: 'PeachPuff',
                        tension: 0.1
                    },
                    ]
                },
                options: {
                    animation: {
                        duration: 0
                    },
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        datalabels: {
                            font: {
                                size: 0
                            }
                        }
                    }
                },
            }
        }
    }
    , methods: {
        newChart(chartData) {
            const ctx = document.getElementById('myChart');
            return new Chart(ctx, chartData);
        },
        updateChart(dataList) {
            if (this.chart) {
                this.chart.destroy();
            }

            if (this.category === "expense" || this.category === "income") {
                Chart.register(ChartDataLabels);
                //若當前模式為支出或收入就顯示圓餅圖
                this.updateCoughnutChartData(dataList);
                this.chart = this.newChart(this.doughnutChartContent);
            } else {
                //若當前模式為盈餘就顯示折線圖
                this.updateLineChartData(dataList)
                this.chart = this.newChart(this.lineChartContent);
            }
        },
        //更新圓餅圖資料內容
        updateCoughnutChartData(dataList) {
            const labelData = dataList.reduce((accumulator, currentValue) => {
                const labelIndex = accumulator.labelNameList.indexOf(currentValue.labelContent.name)
                const isNewLabel = labelIndex == -1;
                if (isNewLabel) {
                    accumulator.labelNameList.push(currentValue.labelContent.name);
                    accumulator.labelValueList.push(currentValue.price);
                    accumulator.totalPrice += currentValue.price
                    const newLabelIndex = accumulator.labelNameList.indexOf(currentValue.labelContent.name)
                    accumulator.colorList.push(this.colorList[newLabelIndex]);
                } else {
                    accumulator.labelValueList[labelIndex] = accumulator.labelValueList[labelIndex] + currentValue.price
                    accumulator.totalPrice += currentValue.price;
                }
                return accumulator;
            }, {
                labelNameList: [],
                labelValueList: [],
                totalPrice: 0,
                colorList: []
            });

            this.doughnutChartContent.data.labels = labelData.labelNameList;
            this.doughnutChartContent.data.datasets[0].data = labelData.labelValueList;
            this.doughnutChartContent.data.totalPrice = labelData.totalPrice;
            this.doughnutChartContent.data.datasets[0].backgroundColor = labelData.colorList;
        },
        //更新折線圖資料內容
        updateLineChartData() {
            this.lineChartContent.data.labels = [];
            this.lineChartContent.data.datasets[0].data = [];
            this.timeClassifiedDataList.forEach(item => {
                //寫入月份名稱
                this.lineChartContent.data.labels.push(item.date)

                //寫入每月盈餘資料
                this.lineChartContent.data.datasets[0].data.push(item.earning)

                //寫入每月支出資料
                this.lineChartContent.data.datasets[1].data.push(item.totalExpense)

                //寫入每月收入資料
                this.lineChartContent.data.datasets[2].data.push(item.totalIncome)
            })
        }
    }
};
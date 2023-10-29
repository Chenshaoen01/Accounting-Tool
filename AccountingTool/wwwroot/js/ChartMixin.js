const ChartMxin = {
    data() {
        return {
            chart: null,
            colorList: ['Blue', 'Orange', 'Yellow', 'Green', 'Red'],
            doughnutChartContent: {
                type: 'doughnut',
                data: {
                    labels: [],
                    datasets: [
                        {
                            label: 'Dataset 1',
                            data: [],
                            backgroundColor: ['Red', 'Orange', 'Yellow', 'Green', 'Blue'],
                        }
                    ]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                    }
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
                        borderColor: 'Orange',
                        tension: 0.1
                        },
                    {
                        label: '每月支出',
                        data: [],
                        fill: false,
                        borderColor: 'Blue',
                        tension: 0.1
                        },
                    {
                        label: '每月收入',
                        data: [],
                        fill: false,
                        borderColor: 'Yellow',
                        tension: 0.1
                    },
                    ]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
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

                    const newLabelIndex = accumulator.labelNameList.indexOf(currentValue.labelContent.name)
                    accumulator.colorList.push(this.colorList[newLabelIndex]);
                } else {
                    accumulator.labelValueList[labelIndex] = accumulator.labelValueList[labelIndex] + currentValue.price
                }
                return accumulator;
            }, {
                labelNameList: [],
                labelValueList: [],
                colorList: []
            });

            this.doughnutChartContent.data.labels = labelData.labelNameList;
            this.doughnutChartContent.data.datasets[0].data = labelData.labelValueList;
            this.doughnutChartContent.data.datasets[0].backgroundColor = labelData.colorList;
        },
        //更新折線圖資料內容
        updateLineChartData() {
            this.lineChartContent.data.labels = [];
            this.lineChartContent.data.datasets[0].data = [];
            this.classifiedDataList.forEach(item => {
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
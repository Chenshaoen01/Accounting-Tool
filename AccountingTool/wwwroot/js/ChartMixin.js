const ChartMxin = {
    data() {
        return {
            chart: null,
            doughnutChartContent: {
                type: 'doughnut',
                data: {
                    labels: ['Red', 'Orange', 'Yellow', 'Green', 'Blue'],
                    datasets: [
                        {
                            label: 'Dataset 1',
                            data: [300, 50, 100, 20, 30],
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
                        title: {
                            display: true,
                            text: 'Chart.js Doughnut Chart'
                        }
                    }
                },
            },
            lineChartContent: {
                type: 'line',
                data: data = {
                    labels: ['Red', 'Orange', 'Yellow', 'Green', 'Blue', 'Black', 'White'],
                    datasets: [{
                        label: 'My First Dataset',
                        data: [65, 59, 80, 81, 56, 55, 40],
                        fill: false,
                        borderColor: 'rgb(75, 192, 192)',
                        tension: 0.1
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Chart.js Line Chart'
                        }
                    }
                },
            }
        }
    }
    , methods: {
        newChart() {
            const ctx = document.getElementById('myChart');
            return new Chart(ctx, this.lineChartContent);
        },
        updateChart() {
            this.chart.destroy();
            this.chart = this.newChart();
            /*this.chart.update();*/
        }
    }
};
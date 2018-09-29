//表格数据
layui.use('table', function () {
    var table = layui.table;
    jQuery.ajax({
        type: "Post",
        url: "GetRecycle",
        data: {},
        success: function (data) {
            var res = JSON.parse(data);
            $("#filecount").text(res.count);
            var newArr = res.data;
            if (newArr.length > 50) {
                //表格
                table.render({
                    elem: '#test'
                    // , url: 'demo.json'
                    , data: newArr
                    , limit: 50
                    , page: {

                        layout: ['count', 'prev', 'page', 'next']
                        , prev: '上一页'
                        , next: '下一页'
                        , count: newArr.length
                        , limit: 50
                        , theme: '#1E9FFF'
                    }
                    , cols: [[
                        { type: 'checkbox' }
                        , { field: 'Name', title: '文件名', templet: '#fileName' }
                        , { field: 'Size', title: '大小' }
                        , { field: 'Date', title: '删除时间' }
                    ]]
                    , done: function (res) {
                        if (res.count == 0) {
                            jQuery("#noData").removeClass("layui-hide");
                            jQuery(".layui-none").remove();
                            jQuery(".layui-table-page").remove();
                        } else {
                            jQuery("#noData").addClass("layui-hide");
                        }
                    }
                });
            } else {
                table.render({
                    elem: '#test'
                    // , url: 'demo.json'
                    , data: newArr
                    , limit: 50

                    , cols: [[
                        { type: 'checkbox' }
                        , { field: 'Name',  title: '文件名', templet: '#fileName' }
                        , { field: 'Size', title: '大小' }
                        , { field: 'Date', title: '删除时间' }
                    ]]
                    , done: function (res) {
                        if (res.count == 0) {
                            jQuery("#noData").removeClass("layui-hide");
                            jQuery(".layui-none").remove();
                            jQuery(".layui-table-page").remove();
                        } else {
                            jQuery("#noData").addClass("layui-hide");
                        }
                    }
                });
            }
            //监听表格复选框选择
            table.on('checkbox(tableTest)', function (obj) {
                console.log(obj)
            });
        }
    })


});
$(function () {
    $("#reduction").off().on("click", function () {
        var table = layui.table;
        var checkStatus = table.checkStatus('test');
        console.log(checkStatus.data)
        var dataArr = [];
        for (var i = 0; i < checkStatus.data.length; i++) {
            dataArr.push(checkStatus.data[i].Shareid)
        }
        if (dataArr.length > 0) {
            $layer.confirm('确认还原吗？', function () {
                $.ajax({
                    type: "Post",
                    url: "Reduction",
                    data: {
                        id: JSON.stringify(dataArr)
                    },
                    success: function (data) {
                        var res = JSON.parse(data);
                        if (res.code == "200") {
                            location.reload();
                        } else {
                            $layer.msg("还原失败");
                        }
                    }
                })
            }, function () {
            })
        } else {
            $layer.msg("请选择文件")
            return;
        }

    })
    $("#empty").off().on("click", function () {
        //询问框
        $layer.confirm('确认清空回收站吗？', function () {
            $.ajax({
                type: "Post",
                url: "empty",
                success: function (data) {
                    var res = JSON.parse(data);
                    if (res.code == "200") {
                        location.reload();
                    } else {
                        $layer.msg("清空失败！")
                    }
                }
            })
        }, function () {
        });
    })
})
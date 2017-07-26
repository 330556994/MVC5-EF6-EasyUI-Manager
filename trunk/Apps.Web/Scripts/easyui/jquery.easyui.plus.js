//删除
function dataDelete(url, datagrid) {
    var row =$('#' + datagrid).datagrid('getSelected');
    if (row != null) {
        $.messager.confirm(index_lang_tip, CommonLang.PleaseSelectTheOperatingRecord, function (r) {
            if (r) {
                $.post("/SysSample/Delete",row, function (data) {
                    if (data.type == 1)
                        $('#' + datagrid).datagrid('load');
                    $.messageBox5s(index_lang_tip, data.message);
                }, "json");

            }
        });
    } else { $.messageBox5s(index_lang_tip, CommonLang.PleaseSelectTheOperatingRecord); }
}
//批量操作
function dataBatchOperater(url, datagrid) {
    var rows = $('#' + datagrid).datagrid('getChecked');
    var ids = "";
    if (rows.length > 0) {
        $.each(rows, function (index, row) {
            ids = ids + row.Id + ",";
        });
        ids = ids.substring(0, ids.length - 1);
        $.post(url + "?id=" + ids, function (data) {
            if (data.type == 1) {
                $('#' + datagrid).datagrid('load');
                $('#' + datagrid).datagrid('clearChecked');
            }
            $.messageBox5s(index_lang_tip, data.message);

        }, "json");
    } else { $.messageBox5s(index_lang_tip, CommonLang.PleaseSelectTheOperatingRecord); }
}



/** 
* 在iframe中调用，在父窗口中出提示框(herf方式不用调父窗口)
*/
$.extend({
    messageBox5s: function (title, msg) {
        $.messager.show({
            title: '<span class="fa fa-info">&nbsp;&nbsp;' + title + '</span>', msg: msg, timeout: 5000, showType: 'slide', style: {
                left: '',
                right: 30,
                top: '',
                bottom: 30,
                width: 250,
                

            }
        });
    }
});
$.extend({
    messageBox10s: function (title, msg) {
        $.messager.show({
            title: '<span class="fa fa-info">&nbsp;&nbsp;' + title + '</span>', msg: msg, height: 'auto', width: 'auto', timeout: 10000, showType: 'slide', style: {
                left: '',
                right: 5,
                top: '',
                bottom: -document.body.scrollTop - document.documentElement.scrollTop + 5
            }
        });
    }
});
$.extend({
    show_alert: function (strTitle, strMsg) {
        $.messager.alert(strTitle, strMsg);
    }
});





/** 
* panel关闭时回收内存，主要用于layout使用iframe嵌入网页时的内存泄漏问题
*/
$.fn.panel.defaults.onBeforeDestroy = function () {

    var frame = $('iframe', this);
    try {
        // alert('销毁，清理内存');
        if (frame.length > 0) {
            for (var i = 0; i < frame.length; i++) {
                frame[i].contentWindow.document.write('');
                frame[i].contentWindow.close();
            }
            frame.remove();
            if ($.browser.msie) {
                CollectGarbage();
            }
        }
    } catch (e) {
    }
};


var oriFunc = $.fn.datagrid.defaults.view.onAfterRender;
$.fn.datagrid.defaults.view.onAfterRender = function (tgt) {
    if ($(tgt).datagrid("getRows").length > 0) {
        
        $(tgt).datagrid("getPanel").find("div.datagrid-body").find("div.datagrid-cell").each(function () {
            var $Obj = $(this)
            $Obj.attr("title", $Obj.text());
        })
    }
};


var oriFunctree = $.fn.treegrid.defaults.view.onAfterRender;
$.fn.treegrid.defaults.view.onAfterRender = function (tgt) {
    if ($(tgt).treegrid("getRoots").length > 0) {
       
        $(tgt).treegrid("getPanel").find("div.datagrid-body").find("div.datagrid-cell").each(function () {
            var $Obj = $(this)
            $Obj.attr("title", $Obj.text());
        })
    }
};

/**
* 防止panel/window/dialog组件超出浏览器边界
* @param left
* @param top
*/

var easyuiPanelOnMove = function (left, top) {
    var l = left;
    var t = top;
    if (l < 1) {
        l = 1;
    }
    if (t < 1) {
        t = 1;
    }
    var width = parseInt($(this).parent().css('width')) + 14;
    var height = parseInt($(this).parent().css('height')) + 14;
    var right = l + width;
    var buttom = t + height;
    var browserWidth = $(window).width();
    var browserHeight = $(window).height();
    if (right > browserWidth) {
        l = browserWidth - width;
    }
    if (buttom > browserHeight) {
        t = browserHeight - height;
    }
    $(this).parent().css({/* 修正面板位置 */
        left: l,
        top: t
    });
};
//$.fn.dialog.defaults.onMove = easyuiPanelOnMove;
//$.fn.window.defaults.onMove = easyuiPanelOnMove;
//$.fn.panel.defaults.onMove = easyuiPanelOnMove;
//让window居中
var easyuiPanelOnOpen = function (left, top) {
    
    var iframeWidth = $(this).parent().parent().width();
   
    var iframeHeight = $(this).parent().parent().height();

    var windowWidth = $(this).parent().width();
    var windowHeight = $(this).parent().height();

    var setWidth = (iframeWidth - windowWidth) / 2;
    var setHeight = (iframeHeight - windowHeight) / 2;
    $(this).parent().css({/* 修正面板位置 */
        left: setWidth,
        top: setHeight
    });

    if (iframeHeight < windowHeight)
    {
        $(this).parent().css({/* 修正面板位置 */
            left: setWidth,
            top: 0
        });
    }
    $(".window-shadow").hide();
    //修复被撑大的问题
    if ($(".window-mask") != null)
    {
        if ($(".window-mask").size() > 1)
        {
            $(".window-mask")[0].remove();
        }
    }
    $(".window-mask").attr("style", "display: block; z-index: 9002; width: " + iframeWidth - 200 + "px; height: " + iframeHeight - 200 + "px;");

    //$(".window-mask").hide().width(1).height(3000).show();
};
$.fn.window.defaults.onOpen = easyuiPanelOnOpen;
var easyuiPanelOnClose = function (left, top) {

  
    $(".window-mask").hide();

    //$(".window-mask").hide().width(1).height(3000).show();
};

$.fn.window.defaults.onClose = easyuiPanelOnClose;
//var easyuiPanelOnResize = function (left, top) {


//    var iframeWidth = $(this).parent().parent().width();

//    var iframeHeight = $(this).parent().parent().height();

//    var windowWidth = $(this).parent().width();
//    var windowHeight = $(this).parent().height();


//    var setWidth = (iframeWidth - windowWidth) / 2;
//    var setHeight = (iframeHeight - windowHeight) / 2;
//    $(this).parent().css({/* 修正面板位置 */
//        left: setWidth-6,
//        top: setHeight-6
//    });

//    if (iframeHeight < windowHeight) {
//        $(this).parent().css({/* 修正面板位置 */
//            left: setWidth,
//            top: 0
//        });
//    }
//    $(".window-shadow").hide();
//    //$(".window-mask").hide().width(1).height(3000).show();
//};
//$.fn.window.defaults.onResize = easyuiPanelOnResize;


/**
* 
* @requires jQuery,EasyUI
* 
* 扩展tree，使其支持平滑数据格式
*/
$.fn.tree.defaults.loadFilter = function (data, parent) {
    var opt = $(this).data().tree.options;
    var idFiled, textFiled, parentField;
    //alert(opt.parentField);
    if (opt.parentField) {
        idFiled = opt.idFiled || 'id';
        textFiled = opt.textFiled || 'text';
        parentField = opt.parentField;
        var i, l, treeData = [], tmpMap = [];
        for (i = 0, l = data.length; i < l; i++) {
            tmpMap[data[i][idFiled]] = data[i];
        }
        for (i = 0, l = data.length; i < l; i++) {
            if (tmpMap[data[i][parentField]] && data[i][idFiled] != data[i][parentField]) {
                if (!tmpMap[data[i][parentField]]['children'])
                    tmpMap[data[i][parentField]]['children'] = [];
                data[i]['text'] = data[i][textFiled];
                tmpMap[data[i][parentField]]['children'].push(data[i]);
            } else {
                data[i]['text'] = data[i][textFiled];
                treeData.push(data[i]);
            }
        }
        return treeData;
    }
    return data;
};

/**

* @requires jQuery,EasyUI
* 
* 扩展combotree，使其支持平滑数据格式
*/
$.fn.combotree.defaults.loadFilter = $.fn.tree.defaults.loadFilter;

//如果datagrid过长显示...截断(格式化时候，然后调用resize事件)
//$.DataGridWrapTitleFormatter("值",$("#List"),"字段");
//onResizeColumn:function(field,width){ var refreshFieldList = ["字段名称","字段名称","字段名称"]; if(refreshFieldList.indexOf(field)>=0){$("#List").datagrid("reload");})}
$.extend({
    DataGridWrapTitleFormatter: function (value,obj,fidld) {
        if (value == undefined || value == null || value == "")
        {
            return "";
        }
        var options = obj.datagrid('getColumnOption', field);
        var cellWidth = 120;
        if (options != undefined) {
            cellWidth = options.width - 10;
        }
        return "<div style='width:" + cellWidth + "px;padding:0px 6px;line-height:25px;height:25px;margin-top:1px;cursor:pointer;white-space:nowrap:overflow:hidden;text-overflow:ellipsis;' title='"+value+"'>"+value+"</div>";
    }
});
//替换字符串
/*
 * 功    能：替换字符串中某些字符
 * 参    数：sInput-原始字符串  sChar-要被替换的子串 sReplaceChar-被替换的新串
 * 返 回 值：被替换后的字符串
 */
$.extend({
    ReplaceStrAll: function (sInput, sChar, sReplaceChar) {
        if (sInput == "" || sInput == undefined) {
            return "";
        }
        var oReg = new RegExp(sChar, "g");
        return sInput.replace(oReg, sReplaceChar);

    }
});

 /*
  * 功    能：替换字符串中某些字符（只能是第一个被替换掉）
  * 参    数：sInput-原始字符串  sChar-要被替换的子串 sReplaceChar-被替换的新串
  * 返 回 值：被替换后的字符串
  */
$.extend({
    ReplaceOne:function (sInput, sChar, sReplaceChar) {
    if (sInput == "" || sInput == undefined) {
                 return "";
     }
         return sInput.replace(sChar, sReplaceChar);
    }
});


function myformatter(date) {
    var dateArray = date.split(" ");
    return dateArray[0].replace("/", "-").replace("/", "-");
}

function myparser(s) {
    if (!s) return new Date();
    var ss = (s.split('-'));
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}

function SetGridWidthSub(w)
{
    return $(window).width() - w;
}
function SetGridHeightSub(h) {
    return $(window).height() - h
}


function SubStrYMD(value)
{
    if (value == null || value == "") {
        return "";
    } else {
        return value.substr(0, value.indexOf(' '))
    }
}
//圆点状态设置，只有蓝和绿色
function EnableFormatter(value)
{
    if (value) {
        return "<span class='label label-success'>启用</span>";
    } else {
        return "<span class='label label-error'>禁用</span>";
    }
}

function CheckFormatter(value) {
    if (value) {
        return "<span class='label label-success'>已审核</span>";
    } else {
        return "<span class='label label-info'>未审核</span>";
    }
}



//ComboTree数据过滤
function queryComboTree(q, comboid, roots) {
    var datalist = [];//过滤后的数据源
    var childrenlist = [];//子节点数据

    $(comboid).combotree('setText', q);
    var entertext = $(comboid).combotree('getText');
    if (entertext == null || entertext == "") {
        //清空值之后重新加载数据
        $(comboid).combotree("loadData", roots).combotree("clear");
        return;
    }
    //循环数组
    for (var i = 0; i < roots.length; i++) {
        var org = {
            'id': roots[i].id,
            'text': roots[i].text,
            'children': []
        };
        if (q.toLowerCase() == roots[i].text.toLowerCase()) {
            $(comboid).combotree('setValue', roots[i].id);
        }
        var childrens = [];//查询到的子节点
        //递归找子节点
        childrensTree(comboid, roots[i], childrens, q);
        if (childrens.length > 0) {
            org.children = childrens;
            datalist.push(org);
        }
        else if (org.text.toLowerCase().indexOf(q.toLowerCase()) >= 0 && org.text != "") {
            //没有子节点，但是根节点符合要求
            datalist.push(org);
        }
    }

    if (datalist.length > 0) {
        $(comboid).combotree("loadData", datalist);
        $(comboid).combotree('setText', q);
        datalist = [];//初始化
        return;
    }

}
//组件ID，根节点，需要填充的数组，查询值
function childrensTree(comboid, roots, datalist, q) {
    var roots = roots.children;
    if (roots != undefined) {
        for (var j = 0; j < roots.length; j++) {
            var org = {
                'id': roots[j].id,
                'text': roots[j].text,
                'children': []
            };
            if (q.toLowerCase() == roots[j].text.toLowerCase()) {
                $(comboid).combotree('setValue', roots[j].id);
            }
            var childrens = [];//查询到的子节点
            //递归找子节点
            childrensTree(comboid, roots[j], childrens, q);
            if (childrens.length > 0) {
                org.children = childrens;
                datalist.push(org);
            }
            else if (org.text.toLowerCase().indexOf(q.toLowerCase()) >= 0 && org.text != "") {
                //没有子节点，但是根节点符合要求
                datalist.push(org);
            }
        }
    }
}

//合并列
//onLoadSuccess: function (data) {
//    $(this).datagrid("autoMergeCells", ['Area', 'PosCode']);
//},
$.extend($.fn.datagrid.methods, {
    autoMergeCells: function (jq, fields) {
        return jq.each(function () {
            var target = $(this);
            if (!fields) {
                fields = target.datagrid("getColumnFields");
            }
            var rows = target.datagrid("getRows");
            var i = 0,
			j = 0,
			temp = {};
            for (i; i < rows.length; i++) {
                var row = rows[i];
                j = 0;
                for (j; j < fields.length; j++) {
                    var field = fields[j];
                    var tf = temp[field];
                    if (!tf) {
                        tf = temp[field] = {};
                        tf[row[field]] = [i];
                    } else {
                        var tfv = tf[row[field]];
                        if (tfv) {
                            tfv.push(i);
                        } else {
                            tfv = tf[row[field]] = [i];
                        }
                    }
                }
            }
            $.each(temp, function (field, colunm) {
                $.each(colunm, function () {
                    var group = this;

                    if (group.length > 1) {
                        var before,
						after,
						megerIndex = group[0];
                        for (var i = 0; i < group.length; i++) {
                            before = group[i];
                            after = group[i + 1];
                            if (after && (after - before) == 1) {
                                continue;
                            }
                            var rowspan = before - megerIndex + 1;
                            if (rowspan > 1) {
                                target.datagrid('mergeCells', {
                                    index: megerIndex,
                                    field: field,
                                    rowspan: rowspan
                                });
                            }
                            if (after && (after - before) != 1) {
                                megerIndex = after;
                            }
                        }
                    }
                });
            });
        });
    }
});
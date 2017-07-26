//全局配置文件。来源于JavascriptServer->Home/ConfigJS
var _YMGlobal;
(function (_YMGlobal) {
    var Config = (function () {
        function Config() {

        }
        Config.currentCulture = "";
        Config.apiUrl = "";
        Config.token = "";
        return Config;
    })();
    _YMGlobal.Config = Config;
})(_YMGlobal || (_YMGlobal = {}));


//要获取的参数名称
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return r[2];
    }
    return null
}
//===========================字符串辅助================================

//生成唯一的GUID
function GetGuid() {
    var s4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    };
    return s4() + s4() + s4() + "-" + s4();
}

//JQGrid行保存后成功返回结果分解对象
function JsonMessage(data) {
    /*
    适用于jQuery('#List').jqGrid('saveRow', id,successfunc(data){});
    */
    var msg = data.responseText.replace(/\"/g, "");
    var msgs = msg.split(',');
    this.type = msgs[0].split(':')[1];
    this.message = msgs[1].split(':')[1];
    this.value = msgs[2].split(':')[1];
    this.obj = msgs[3].split(':')[1];
};

//格式化日期2012-9-10 10:25:14=>2012-9-10
function DateFormat(name) {
    var date = $("#" + name).val();
    var dateArray = date.split(" ");
    $("#" + name).val(dateArray[0].replace("/","-").replace("/", "-"));
}
//去掉最后一个逗号
function RemoveLastChar(str) {
    return str.substring(0, receiver.length - 1);
}
//根据字符将字符串分解成数组
function AnalyzeArr(str) {
    var arr_id = new Array();
    arr_id = str.split(",");
    return arr_id
}
/**
*取得字符的字节长度（汉字占2，字母占1）
*
*/
function strLen(s) {
    var len = 0;
    for (var i = 0; i < s.length; i++) {

        if (!ischinese(s.charAt(i))) {
            len += 1;
        } else {
            len += 2;
        }
    }
    return len;

}
/**
*判断是否中文函数
*/
function ischinese(s) {
    var ret = false;

    for (var i = 0; i < s.length; i++) {
        if (s.charCodeAt(i) >= 256) {
            ret = true;
            break;
        }
    }

    return ret;
}

//===========================系统管理JS函数================================
//Tab控制函数
function customTabs(tabId, tabNum) {
    //设置点击后的切换样式
    $(tabId + " .tab_nav li").removeClass("selected");
    $(tabId + " .tab_nav li").eq(tabNum).addClass("selected");
    //根据参数决定显示内容
    $(tabId + " .tab_con").hide();
    $(tabId + " .tab_con").eq(tabNum).show();
}

//接收人,接收人名称，信息 组需要加_ 
//返回信息ID
function SendMessage(receiver, receiverTitle, mes) {
    var returnResult = showModalDialog("/MIS/WebIM/CeleritySend", [receiver, receiverTitle, mes], "dialogLeft:150px;dialogTop:200px;dialogwidth:726px; dialogheight:206px;");
    return returnResult;
}
//返回选择人员列表
//返回人员ID列表   人员ID:^人员名称" ,格式：a,b,_c^[a],[b],[+c]
//没有选择返回一个""
function SelMemberList() {
    var value = 1;
    var returnResult = showModalDialog("/MIS/WebIM/SelMember", [value], "dialogLeft:150px;dialogTop:200px;dialogwidth:420px; dialogheight:440px;");
    return returnResult;
}
//================上传文件JS函数开始，需和jquery.form.js一起使用===============
//文件上传
function Upload(action, repath, uppath, iswater, isthumbnail,form) {

    var sendUrl = "/Core/upload_ajax.ashx?action=" + action + "&ReFilePath=" + repath + "&UpFilePath=" + uppath;
    //判断是否打水印
    if (arguments.length == 4) {
        sendUrl = "/Core/upload_ajax.ashx?action=" + action + "&ReFilePath=" + repath + "&UpFilePath=" + uppath + "&IsWater=" + iswater;
    }
    //判断是否生成宿略图
    if (arguments.length == 5 || arguments.length == 6) {
        //不是图片
        if (iswater != "" && isthumbnail != "")
        {
            sendUrl = "/Core/upload_ajax.ashx?action=" + action + "&ReFilePath=" + repath + "&UpFilePath=" + uppath + "&IsWater=" + iswater + "&IsThumbnail=" + isthumbnail;
        }
    }
    if (form == undefined || form == "")
    {
        form = "form";
    }
    //开始提交
    $(form).ajaxSubmit({
        beforeSubmit: function (formData, jqForm, options) {
            //隐藏上传按钮
            $("#" + repath).nextAll(".files").eq(0).hide();
            //显示LOADING图片
            $("#" + repath).nextAll(".uploading").eq(0).show();
        },
        success: function (data, textStatus) {
            if (data.msg == 1) {
                
                $("#" + repath).val(data.msgbox.split(",")[0]);
                
                $("#" + repath).next("img").attr("src", data.msgbox.split(",")[0]);
            } else {
                alert(data.msgbox);
            }
            $("#" + repath).nextAll(".files").eq(0).show();
            $("#" + repath).nextAll(".uploading").eq(0).hide();
        },
        error: function (data, status, e) {
            alert("上传失败，错误信息：" + e);
            $("#" + repath).nextAll(".files").eq(0).show();
            $("#" + repath).nextAll(".uploading").eq(0).hide();
        },
        url: sendUrl,
        type: "post",
        dataType: "json",
        timeout: 600000
    });
};
//附件上传
function AttachUpload(repath, uppath) {
    var submitUrl = "/Core/upload_ajax.ashx?action=AttachFile&UpFilePath=" + uppath;
    //开始提交
    $("form").ajaxSubmit({
        beforeSubmit: function (formData, jqForm, options) {
            //隐藏上传按钮
            $("#" + uppath).parent().hide();
            //显示LOADING图片
            $("#" + uppath).parent().nextAll(".uploading").eq(0).show();
        },
        success: function (data, textStatus) {
            if (data.msg == 1) {
                var listBox = $("#" + repath + " ul");
                var newLi = '<li>'
                + '<input name="hidFileName" type="hidden" value="0|' + data.mstitle + "|" + data.msgbox + '" />'
                + '<b class="close" title="删除" onclick="DelAttachLi(this);"></b>'
                + '<span class="right">下载积分：<input name="txtPoint" type="text" class="input2" value="0" onkeydown="return checkNumber(event);" /></span>'
                + '<span class="title">附件：' + data.mstitle + '</span>'
                + '<span>人气：0</span>'
                + '<a href="javascript:;" class="upfile"><input type="file" name="FileUpdate" onchange="AttachUpdate(\'hidFileName\',this);" /></a>'
                + '<span class="uploading">正在更新...</span>'
                + '</li>';
                listBox.append(newLi);
                //alert(data.mstitle);
            } else {
                alert(data.msgbox);
            }
            $("#" + uppath).parent().show();
            $("#" + uppath).parent().nextAll(".uploading").eq(0).hide();
        },
        error: function (data, status, e) {
            alert("上传失败，错误信息：" + e);
            $("#" + uppath).parent().show();
            $("#" + uppath).parent().nextAll(".uploading").eq(0).hide();
        },
        url: submitUrl,
        type: "post",
        dataType: "json",
        timeout: 600000
    });
};
//更新附件上传
function AttachUpdate(repath, uppath) {
    var btnOldName = $(uppath).attr("name");
    var btnNewName = "NewFileUpdate";
    $(uppath).attr("name", btnNewName);
    var submitUrl = "/Core/upload_ajax.ashx?action=AttachFile&UpFilePath=" + btnNewName;
    //开始提交
    $("form").ajaxSubmit({
        beforeSubmit: function (formData, jqForm, options) {
            //隐藏上传按钮
            $(uppath).parent().hide();
            //显示LOADING图片
            $(uppath).parent().nextAll(".uploading").eq(0).show();
        },
        success: function (data, textStatus) {
            if (data.msg == 1) {
                var ArrFileName = $(uppath).parent().prevAll("input[name='" + repath + "']").val().split("|");
                $(uppath).parent().prevAll("input[name='" + repath + "']").val(ArrFileName[0] + "|" + data.mstitle + "|" + data.msgbox);
                $(uppath).parent().prevAll(".title").html("附件：" + data.mstitle);
            } else {
                alert(data.msgbox);
            }
            $(uppath).parent().show();
            $(uppath).parent().nextAll(".uploading").eq(0).hide();
            $(uppath).attr("name", btnOldName);
        },
        error: function (data, status, e) {
            alert("上传失败，错误信息：" + e);
            $(uppath).parent().show();
            $(uppath).parent().nextAll(".uploading").eq(0).hide();
            $(uppath).attr("name", btnOldName);
        },
        url: submitUrl,
        type: "post",
        dataType: "json",
        timeout: 600000
    });
};
//===========================上传文件JS函数结束================================


//===========================计算辅助================================
//保留2位小数 3.14159 =3.14
function changeTwoDecimal(x) {
    if (x == "Infinity") {
        return;
    }
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        return
    } else {
        var f_x = Math.round(x * 100) / 100;
        var s_x = f_x.toString();
        var pos_decimal = s_x.indexOf('.');
        if (pos_decimal < 0) {
            pos_decimal = s_x.length;
            s_x += '.';
        }
        while (s_x.length <= pos_decimal + 2) {
            s_x += '0';
        }
        return s_x;
    }
}
function isDate_yyyyMMdd(str) {
    var reg = /^([0-9]{1,4})(-|\/)([0-9]{1,2})\2([0-9]{1,2})$/;
    var r = str.match(reg);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4]);
    var newstr = d.getFullYear() + r[2] + (d.getMonth() + 1) + r[2] + d.getDate();
    var yyyy = parseInt(r[1], 10);
    var mm = parseInt(r[3], 10);
    var dd = parseInt(r[4], 10);
    var compstr = yyyy + r[2] + mm + r[2] + dd;
    return newstr == compstr;
}
//===========================上传文件JS函数结束================================
//是否存在指定函数 
function isExitsFunction(funcName) {
    try {
        if (typeof (eval(funcName)) == "function") {
            return true;
        }
    } catch (e) { }
    return false;
}
//是否存在指定变量 
function isExitsVariable(variableName) {
    try {
        if (typeof (variableName) == "undefined") {
            //alert("value is undefined"); 
            return false;
        } else {
            //alert("value is true"); 
            return true;
        }
    } catch (e) { }
    return false;
}
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function active(id) {
  $("#" + id).addClass("active");
}

function loadStudentsList(page) {
  id = $(".tab-slider--trigger.active").prop("id");
  sort = null;
  order = null;
  tab = $("#" + id);

  if ($(".fa-fw.fa-sort-up").length == 1) {
    sort = $(".fa-fw.fa-sort-up")
      .attr("class")
      .split(" ")[0];
    order = "asc";
  } else if ($(".fa-fw.fa-sort-down").length == 1) {
    sort = $(".fa-fw.fa-sort-down")
      .attr("class")
      .split(" ")[0];
    order = "desc";
  }

  keyword = $("#searchStudents").val();
  limit = parseInt($("#limit").val());
  if (id == "tabCard") {
    limit += 1;
  }
  offset = page == 1 ? 0 : (page - 1) * limit;

  $.ajax({
    type: "GET",
    url: "Home/LoadStudent",
    data: {
      keyword: keyword,
      limit: limit,
      offset: offset || 0,
      sort: sort,
      order: order
    },
    dataType: "json",
    error: function(resp) {
      $("#studentsTable").html(resp.responseText);
      countTotal(keyword, limit, page || 1);
      setIcon(sort, order);
      changeTab(tab);
    }
  });
}

function countTotal(keyword, limit, page) {
  $.ajax({
    type: "GET",
    url: "Home/LoadStudent",
    data: { keyword: keyword, limit: limit, offset: 0, count: true },
    dataType: "json",
    success: function(resp) {
      paginationStudents(page, resp, limit);
    }
  });
}

function paginationStudents(pageSelected, total, limit) {
  page = "";
  total = Math.ceil(total / limit);
  for (var i = 0; i <= total - 1; i++) {
    page = i + 1;
    offset = page == 1 ? 0 : (page - 1) * limit;
    active = pageSelected == page ? "active" : "";
    $("#paginationStudents").append(
      '<li class="page-item ' +
        active +
        '" id="' +
        page +
        '" ><a class="page-link cicle" onclick="loadStudentsList(' +
        page +
        ')" href="javascript:void(null)">' +
        page +
        "</a></li>"
    );
  }
}

function removeStudent(id) {
  var confirmation = confirm("are you sure you want to remove the item?");
  if (confirmation) {
    $.ajax({
      type: "Delete",
      url: "Home/Delete/",
      data: { id: id },
      dataType: "json",
      success: function(resp) {
        if (resp) {
          loadStudentsList(parseInt($(".page-item.active").prop("id")));
        }
      }
    });
  }
}

function sortStudent(sort) {
  page = parseInt($(".page-item.active").prop("id"));
  icon = $("." + sort);
  if (icon.hasClass("fa-sort")) {
    resetIcon();
    icon.removeClass("fa-sort");
    icon.addClass("fa-sort-up");
    icon.addClass("text-success");
    loadStudentsList(page);
  } else if (icon.hasClass("fa-sort-up")) {
    resetIcon();
    icon.removeClass("fa-sort-up");
    icon.addClass("fa-sort-down");
    icon.addClass("text-success");
    loadStudentsList(page);
  } else {
    resetIcon();
    loadStudentsList(page);
  }
}

function resetIcon() {
  moreIcon = $(".fa-fw");
  icon.removeClass("text-success");
  moreIcon.removeClass("fa-sort-up");
  moreIcon.removeClass("fa-sort-down");
  moreIcon.addClass("fa-sort");
}

function setIcon(sort, order) {
  icon = $("." + sort);
  if (order == "asc") {
    icon.removeClass("fa-sort");
    icon.addClass("fa-sort-up");
    icon.addClass("text-success");
  } else if (order == "desc") {
    icon.removeClass("fa-sort-up");
    icon.addClass("fa-sort-down");
    icon.addClass("text-success");
  } else {
    resetIcon();
  }
}

function changeTab(self) {
  $(".tab-slider--body").hide();
  var activeTab = $(self).attr("rel");
  $("#" + activeTab).fadeIn();
  if ($(self).attr("rel") == "tab2") {
    $(".tab-slider--tabs").addClass("slide");
  } else {
    $(".tab-slider--tabs").removeClass("slide");
  }
  $(".tab-slider--nav li").removeClass("active");
  $(self).addClass("active");
}

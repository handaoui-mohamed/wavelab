var ps = getParams(window.location.href), $bmIcon;

function callBack() {
  if ( !ps.alone && !ps.print ) {
      if(ps.hash && ps.hash.match(/^#\d+$/)){
        parent.location.replace( "index.html#!" + ps.hash.match(/\d+$/) );
      }
      else{
        parent.location.replace( "index.html#!" + pid );
      }
  } 
}

if ( !ps.alone && !ps.print && parent == window ) {
    callBack();
}
      

var bm = new Bookmarks( "Bookmarks_0188D3AD5B7730409C63C25989939B3B" );
function toggleBookmark() {
    bm.toggle( pid );
    updateBmIcon();
    if ( parent != window && parent.updateBookmarks ) {
        parent.updateBookmarks();
    }
}
function updateBmIcon() {
    if ( bm.check( pid ) ) {
        $bmIcon.addClass( "ui-state-highlight" );
    } else {
        $bmIcon.removeClass( "ui-state-highlight" );
    }
}

$( function(){

    var $topic = $( "#topic" );
    hl_class="ui-state-highlight";
    onLive();
    callBack();
          
    if ( parent == window ) {
        $( "#alone-help-icon" ).hide();
    } else {
        $( "#full-help-icon" ).hide();
    }
    $bmIcon = $( "#bookmark_icon1" );
    if ( window.location.protocol == "file:" ) {
        $bmIcon.parent().hide();
        var $icons = $( "#icons-right" );
        $icons.width( $icons.width() - 24 );
    } else {
        updateBmIcon();
    }

  if ( ps.alone ) {
      $( "a[href]" ).each( function(){
          var $this = $(this);
          if ( !$this.attr("id") ) {
            $this.attr( "href", $this.attr("href") + "#!-alone" );
          }
      });
  }


    $( ".sub-title" ).each( function(){
        var $this = $( this );
        var $title = $this.children( ":first" ).addClass( "heading-fix" );
        var c = $this.attr("empty") == "True" ? "" : "collapsable";
        $title.html( 
            '<span class="sub-title-inner {1} expanded ui-state-active">{0}&nbsp;</span>'.format( $title.html(), c ) 
        );
        if ( c == "" ) {
            $this.next("div").hide();
        }
    });
    var $clickables = $( ".collapsable,.button,.icon1" ).css( "cursor", "pointer" )
    .mouseenter( function(){
        var $this = $( this ).addClass( "ui-state-hover" );
        if ( $this.hasClass("collapsable") ) {
            $this.removeClass( "ui-state-active" );
        }
    })
    .mouseleave( function(){
        var $this = $(this).removeClass( "ui-state-hover" );
        if ( $this.hasClass("collapsable") ) {
            $this.addClass( "ui-state-active" );
        }
    });
    var $titles = $clickables.filter( ".collapsable" ).click( function(){
        var $this = $(this);
        var $div = $this.parent().parent().next( "div" );
        if ( $div.is(":visible") ) {
            $this.append( '<span class="sub-title-more ui-state-highlight">...</span>' )
                 .removeClass( "expanded" )
                 .addClass( "collapsed" );
        } else {
            $this.removeClass( "ui-state-hover collapsed")
                 .addClass( "expanded ui-state-active" )
                 .find( ".sub-title-more" ).remove();
        }
        $div.slideToggle( "fast" );
    });
    $( "#collapse-all" ).click( function(){
        $titles.filter( ".expanded" ).click();
    });
    $( "#expand-all" ).click( function(){
        $titles.filter( ".collapsed" ).click();
    });
    $clickables.filter( ".to-top" ).click( function(){
        $topic.scrollTop( 0 );
    });
    if ( ps.hash ) {
        var expr = "a[name={0}]".format( ps.hash.substring(1, ps.hash.length) );
        var a = $( expr ).get(0);
        if (a && a.scrollIntoView) {
            a.scrollIntoView(true);
        }
    }

    function resizeIE6() {
        fixIE6Position( "crumbs,topic" );
    }
    var __windowWidth, __windowHeight;
    function checkWindowResize() { 
        if ( __windowHeight != document.documentElement.clientHeight || __windowWidth != document.documentElement.clientWidth ) {
            __windowHeight = document.documentElement.clientHeight;
            __windowWidth  = document.documentElement.clientWidth;
            resizeIE6();
        }
    }
    if ( window.isIE6 ) {
        setInterval(checkWindowResize, 200);
        resizeIE6();
    }

    $topic.css("bottom", $topic.css("bottom") ); // fix ie issue
});

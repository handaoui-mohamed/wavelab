$( function() {

    var $tree, $index, $bookmark, $search, $keyword, lastSearch,
        activeTab = null,
        lastIdx = -1,
        topicCount = data.tl.length,
        title = document.title;

    var $tabsWrapper = $( "#tabs" );
    $tabsWrapper.find( ".toc-body" ).hide();
    
    var $tabs = $tabsWrapper.find( ".ui-tab" )
    .mouseenter( function(){
        $( this ).addClass( "ui-state-hover" );
    })
    .mouseleave( function(){
        $( this ).removeClass( "ui-state-hover" );
    });
    if ( window.location.protocol != "file:" ) {
        $tabs.filter( "[index=b]" ).show();
    }

    $tabs.selectTab = function( index ) {
        var $tab = $tabs.filter( ".ui-tabs-selected" )
        .removeClass( "ui-tabs-selected ui-state-active" );
        if ( $tab.length > 0 ) {
          $tabsWrapper.find( $tab.attr("tab") ).hide();
        }
        
        $tab = $tabs.filter( "[index={0}]".format(index) ).show();
        $tab.addClass( "ui-tabs-selected ui-state-active" );
        var $div = $tabsWrapper.find( $tab.attr("tab") ).show();
        initTab( index );
    }

    $tabs.click( function(){
        $tabs.selectTab( $(this).attr("index") );
        return false;
    });
    
    function initTab( index ) {
        if( index == "t" && !$tree ) {
            $tree = $("#tabs-0")
            .moTree( data, {
                target: "body-frame",
                hoverClass: "ui-state-hover",
                selectedClass: "ui-state-highlight",
                nodeTooltip: true,
                autoSelectNonEmpty: true
            })
;
        } else if ( index == "i" && !$index ) {
            $index = $("#tabs-1")
            .moTree( IndexData(false, true), {
                target: "body-frame",
                hoverClass: "ui-state-hover",
                selectedClass: "ui-state-highlight"
            })
;
        } else if ( index == "b" && !$bookmark ) {
            updateBookmarks();
        }
        activeTab = index;
        if ( activeTab == "t" || activeTab == "i" ) {
            $( ".tree-tool" ).show();
        } else {
            $( ".tree-tool" ).hide();
        }
    }

    var bm = new Bookmarks( "Bookmarks_0188D3AD5B7730409C63C25989939B3B" );
    window.updateBookmarks = updateBookmarks;
    function updateBookmarks() {
        var items = bm.toArray();
        
        var idx = 0, sData = {ns:[], ps:[], ts:[], tl:[]};
        for( var i=0; items && i<items.length; i++ ) {
            var r = findId( items[i] );
            if ( r >= 0 ) {
                var url = data.ps[ r ], title = data.ts[ r ];
                sData.ps[idx] = url;
                sData.ts[idx] = title;
                sData.tl[idx] = 1;
                idx ++;
            }
        }
        if( idx > 0 ) {
            $bookmark = $("#tabs-2")
            .moTree( sData, {
                target: "body-frame",
                hoverClass: "ui-state-hover",
                selectedClass: "ui-state-highlight"
            });
        } else {
            $("#tabs-2").html( "No bookmarks." );
        }
    }

    function showTab( index ) {
        $tabs.selectTab( index );
    }

    function loadTopic( url ) {
        if ( !url.match( /#/ ) ) {
            url += "#!"; // do not reload same page!
        }
        $( "#body-frame" ).get(0).contentWindow.location.replace( url );
    }

    data.showPage = function( idx, url, ps ) { // idx = node.props.idx
        if ( idx < 0 ) {
            return;
        }
        lastIdx = idx;
        if ( activeTab == null ) {
            showTab( "t" );
        }
        var href = data.ps[idx];
        if ( ps && ps.hl ){
            href += "#!hl=" + ps.hl;
        } else if ( ps && ps.highlight ){
            href += "#!hl=" + ps.highlight;
        }
        if(ps.hash) href += ps.hash;
        loadTopic( href );
        if ( $tree ) {
            $tree.selectNodeByIdx( idx );
            document.title = $tree.$selectedNode.text() + " - " + title;
        }
    }
    
    data.showIndex = function( idx, ps ) { // idx = node.props.idx
        showTab( "i" );
        if( idx != null ) {
            $index.selectNodeByIdx( idx );
        }
    }
    
    data.showSearch = function( args, ps ) {
        showTab( "s" );
        if( args != lastSearch ) {
            $( "#search-frame" ).get(0).contentWindow.location.replace( "_search.html" + args ); // not add to history
            lastSearch = args;
        }
        $keyword.val( ps.search );
    }
    
    $keyword = $( "#search-keyword" ).keypress( function( event ) {
        if( event.keyCode==13 ) {
            window.location.href = "#!search=" + $( this ).val();
        } 
    });
    $( "#search-button" ).click( function() {
        window.location.href = "#!search=" + $keyword.val();
        return false;
    });

    $( ".button" ).css( "cursor", "pointer" )
    .mouseenter( function(){
        $( this ).addClass( "ui-state-hover" );
    })
    .mouseleave( function(){
        $(this).removeClass( "ui-state-hover" );
    });
    $( ".expand-all" ).click( function(){
        if ( activeTab == "t" ) {
            $tree.expandAll();
        } else if ( activeTab == "i" ) {
            $index.expandAll();
        }
        return false;
    });
    $( ".collapse-all" ).click( function(){
        if ( activeTab == "t" ) {
            $tree.collapseAll();
        } else if ( activeTab == "i" ) {
            $index.collapseAll();
        }
        return false;
    });

    $("body").layout({
          top:    { id:"#header", left:2,   right:2,   top:2,    height: 74 }
        , bottom: { id:"#footer", left:2,   right:2,   bottom:2, height: 20 }
        , left:   { id:"#toc",    left:2,   width:316, top:80,   bottom: 26 }
        , right:  { id:"#body",   left:322, right:2,   top:80,   bottom: 26 }
        , sp:     { id:"#sp",     left:318, width:6,   top:80,   bottom: 26 }
        , ov:     { id:"#sp-ov" }
        , spclass:"ui-widget-header active"
    });

    function resizeIE6() {
        fixIE6Position( "header,footer,toc,body,sp,sp-ov,tabs,tabs-0,tabs-1,tabs-2,tabs-3" );
    }
    if ( window.isIE6 ) {
        window.onresize = resizeIE6;
        resizeIE6();
    }

    onLive( Go );
    
});

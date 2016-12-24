<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<!DOCTYPE html>
<html lang="en">

<head>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Tweet</title>

    <!-- Bootstrap Core CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom CSS -->
    <link href="css/stylish-portfolio.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="http://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,700,300italic,400italic,700italic" rel="stylesheet" type="text/css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    
    
     
</head>

<body onload="initialize()">

    <!-- Header -->
    <header id="top" class="header">
        <div class="text-vertical-center">
            <h1>a-Seek Tweet</h1>
            <h3>Temukan Apa yang Orang Bandung Katakan Terkait Pemerintah</h3>
            <br>
            <a href="#about" class="btn btn-light btn-lg">Mulai</a>
        </div>
    </header>

    <div id="googlemap"> </div>
    <!-- About -->
    <section id="about" class="about">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <div class = "panel panel-primary">
                       <div class = "panel-heading">
                          <h3 class = "panel-title">Mulai Mencari</h3>
                        </div>
                        <div class = "panel-body">
                            
                            <form runat="server">
                              Pencarian Twitter:<br>
                            
                               <asp:TextBox ID="keyTwit" runat="server" CssClass="form-control input-lg" placeholder="contoh: #pemkotbdg"></asp:TextBox>
                                    
                              <br>
                              <br>
                              Kata Kunci Dinas Kebersihan:<br>
                               
                              <asp:TextBox ID="keyKebersihan" runat="server" CssClass="form-control input-lg " placeholder="contoh: sampah,bau,kotor,dll  (dipisahkan dengan koma tanpa spasi)"></asp:TextBox><br>
                                    
                              <br>
                              Kata Kunci Dinas Pemakaman dan Pertamanan:<br>
                              
                              <asp:TextBox ID="keyPertamanan" runat="server" CssClass="form-control input-lg " placeholder="contoh: taman,dll (dipisahkan dengan koma tanpa spasi)"></asp:TextBox><br>
                                   
                              <br>
                              Kata Kunci Dinas Pendidikan:<br>
                               
                              <asp:TextBox ID="keyPendidikan" runat="server" CssClass="form-control input-lg " placeholder="contoh: sekolah,kampus,bolos,dll (dipisahkan dengan koma tanpa spasi)"></asp:TextBox><br>
                                    
                              <br>
                              Kata Kunci Dinas Perhubungan:<br>
                               
                              <asp:TextBox ID="keyPerhubungan" runat="server" CssClass="form-control input-lg " placeholder="contoh: jalan,macet,dll (dipisahkan dengan koma tanpa spasi)"></asp:TextBox><br>
                                   
                              <br>
                              Kata Kunci Dinas Sosial:<br>
                                
                              <asp:TextBox ID="keySosial" runat="server" CssClass="form-control input-lg " placeholder="contoh: gelandangan,pengemis,dll (dipisahkan dengan koma tanpa spasi)"></asp:TextBox><br>
                                 
                            <br>
                                
                              Algoritma : <br>
                               <asp:RadioButtonList ID="algoritma" runat="server">
                                    <asp:ListItem Text="Boyer-Moore" Value="boyermor" Selected="true"/>
                                    <asp:ListItem Text="Knuth-Morris-Pratt" Value="kmp" />
                                </asp:RadioButtonList>
                             <br>
                             <br>
                            
                             <asp:Button Text="Jalankan" CssClass="btn btn-dark btn-lg" OnClick="seekTwit" runat="server" />
                         </form>
                       </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Tweet dari Dinas Kebersihan</div>
                        <div class="panel-body">
                            <p>
                           <asp:Label ID="dinasKebersihan" runat="server">
                               Halo!
                           </asp:Label>
                                </p>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Tweet dari Dinas Pemakaman dan Pertamanan</div>
                        <div class="panel-body">
                            <p>
                            <asp:Label ID="dinasPertamanan" runat="server">
                              Kami
                           </asp:Label></p>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Tweet dari Dinas Pendidikan</div>
                        <div class="panel-body">
                            <p>
                            <asp:Label ID="dinasPendidikan" runat="server">
                               Adalah
                           </asp:Label></p>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Tweet dari Dinas Perhubungan</div>
                        <div class="panel-body">
                            <p>
                            <asp:Label ID="dinasPerhubungan" runat="server">
                               A
                           </asp:Label></p>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Tweet dari Dinas Sosial</div>
                        <div class="panel-body">
                            <p>
                            <asp:Label ID="dinasSosial" runat="server">
                               Seek
                           </asp:Label></p>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Tweet Tidak Terkategorisasi</div>
                        <div class="panel-body">
                            <p>
                            <asp:Label ID="dinasUnknown" runat="server">
                               Tweet!!
                           </asp:Label></p>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Lokasi yang Terdeteksi</div>
                        <div class="panel-body">
                            <p>
                            <asp:Label ID="lokasi" runat="server">
                               ....
                           </asp:Label></p>
                        </div>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">Melihat Lokasi pada Peta</div>
                        <div class="panel-body">
                            <p>
                            <asp:Label ID="peta" runat="server">
                               <h2>Google Map Address</h2>

    
                                <input type="text" id="in_address" />
                                <button id="getAddress" onclick="updateMarker()" > submit </button><br>
                                <center>
                                <div id="map_canvas" style="width:710px; height:300px"></div>
                                </center?
                           </asp:Label></p>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.row -->
        </div>
        <!-- /.container -->
    </section>

    


    <!-- Services -->
    <!-- The circle icons use Font Awesome's stacked icon classes. For more information, visit http://fontawesome.io/examples/ -->
    <section id="services" class="services bg-primary">
        <div class="container">
            <div class="row text-center">
                <div class="col-lg-10 col-lg-offset-1">
                    <h2>Our Team</h2>
                    <hr class="small">
                    <div class="row">
                        <div class="col-md-4 col-sm-6">
                            <div class="service-item">
                                
                                <h4>
                                    <strong>I Dewa Putu Deny Krisna Amrita</strong>
                                </h4>
                                <p>13514096</p>
                                <a href="https://www.facebook.com/dny.krs" class="btn btn-light">Connect</a>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-6">
                            <div class="service-item">
                               
                                <h4>
                                    <strong>Scarletta Julia Yapfrine</strong>
                                </h4>
                                <p>13514074</p>
                                <a href="https://www.facebook.com/ScarlettaJulia" class="btn btn-light">Connect</a>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-6">
                            <div class="service-item">
                                
                                <h4>
                                    <strong>Wiega Sonora</strong>
                                </h4>
                                <p>13514019</p>
                                <a href="https://www.facebook.com/wiegasonora" class="btn btn-light">Connect</a>
                            </div>
                        </div>
                    </div>
                    <!-- /.row (nested) -->
                </div>
                <!-- /.col-lg-10 -->
            </div>
            <!-- /.row -->
        </div>
        <!-- /.container -->
    </section>



    <!-- Footer -->
    <footer>
        <div class="container">
            <div class="row">
                <div class="col-lg-10 col-lg-offset-1 text-center">
                    <h4><strong>Start Bootstrap</strong>
                    </h4>
                    <p>3481 Melrose Place<br>Beverly Hills, CA 90210</p>
                    <ul class="list-unstyled">
                        <li><i class="fa fa-phone fa-fw"></i> (123) 456-7890</li>
                        <li><i class="fa fa-envelope-o fa-fw"></i>  <a href="mailto:name@example.com">name@example.com</a>
                        </li>
                    </ul>
                    <br>
                    <p> Photo Header by : berjalanjalan.com </p>
                    <ul class="list-inline">
                        <li>
                            <a href="#"><i class="fa fa-facebook fa-fw fa-3x"></i></a>
                        </li>
                        <li>
                            <a href="#"><i class="fa fa-twitter fa-fw fa-3x"></i></a>
                        </li>
                        <li>
                            <a href="#"><i class="fa fa-dribbble fa-fw fa-3x"></i></a>
                        </li>
                    </ul>
                    <hr class="small">
                    <p class="text-muted">Copyright &copy; Your Website 2014</p>
                </div>
            </div>
        </div>
    </footer>

    <!-- jQuery -->
    <script src="js/jquery.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="js/bootstrap.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script>
    

    // Scrolls to the selected menu item on the page
    $(function() {
        $('a[href*=#]:not([href=#])').click(function() {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') || location.hostname == this.hostname) {

                var target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    $('html,body').animate({
                        scrollTop: target.offset().top
                    }, 1000);
                    return false;
                }
            }
        });
    });
    </script>

    <script src="http://code.jquery.com/jquery-latest.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>

    <script type="text/javascript">
        function updateMarker() {
            var geocoder = new google.maps.Geocoder();

            var address = document.getElementById("in_address").value;

            geocoder.geocode({ 'address': address }, function (results, status) {

                if (status == google.maps.GeocoderStatus.OK) {
                    var latitude = results[0].geometry.location.lat();
                    var longitude = results[0].geometry.location.lng();


                    initialize(latitude, longitude);

                }

            });
        }

    function initialize(latitude,longitude) {
        var latlng = new google.maps.LatLng(latitude,longitude);

        var myOptions = {
          zoom: 14,
          center: latlng,
          mapTypeId: google.maps.MapTypeId.ROADMAP,
          mapTypeControl: false
        };
        var map = new google.maps.Map(document.getElementById("map_canvas"),myOptions);

        var marker = new google.maps.Marker({
          position: latlng,
          animation: google.maps.Animation.BOUNCE
        });
        marker.setMap(map);
        
      }


    </script>
    

</body>

</html>

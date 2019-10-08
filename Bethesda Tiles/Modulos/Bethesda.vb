Imports Microsoft.Toolkit.Uwp.Helpers
Imports Microsoft.Toolkit.Uwp.UI.Animations
Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Xaml.Media.Animation

Module Bethesda

    Public Async Sub Generar()

        Dim helper As New LocalObjectStorageHelper

        Dim recursos As New Resources.ResourceLoader()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim spProgreso As StackPanel = pagina.FindName("spProgreso")
        spProgreso.Visibility = Visibility.Visible

        Dim tbProgreso As TextBlock = pagina.FindName("tbProgreso")
        tbProgreso.Text = String.Empty

        Dim botonCache As Button = pagina.FindName("botonConfigLimpiarCache")
        botonCache.IsEnabled = False

        Dim gv As GridView = pagina.FindName("gridViewTiles")
        gv.Items.Clear()

        Dim listaJuegos As New List(Of Tile)

        If Await helper.FileExistsAsync("juegos") = True Then
            listaJuegos = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")
        End If

        Dim listaBBDD As List(Of BethesdaBBDDEntrada) = BethesdaBBDD.Listado

        Dim i As Integer = 0
        For Each juegoBBDD In listaBBDD
            Dim añadir As Boolean = True
            Dim g As Integer = 0
            While g < listaJuegos.Count
                If listaJuegos(g).ID = juegoBBDD.IDBethesda Then
                    añadir = False
                End If
                g += 1
            End While

            If añadir = True Then
                Dim imagenIcono As String = String.Empty

                Try
                    imagenIcono = Await Cache.DescargarImagen(Await SacarIcono(juegoBBDD.IDSteam), juegoBBDD.IDSteam, "icono")
                Catch ex As Exception

                End Try

                Dim imagenLogo As String = String.Empty

                Try
                    imagenLogo = Await Cache.DescargarImagen("https://steamcdn-a.akamaihd.net/steam/apps/" + juegoBBDD.IDSteam + "/logo.png", juegoBBDD.IDSteam, "logo")
                Catch ex As Exception

                End Try

                Dim imagenAnchaReducida As String = String.Empty

                Try
                    imagenAnchaReducida = Await Cache.DescargarImagen("https://steamcdn-a.akamaihd.net/steam/apps/" + juegoBBDD.IDSteam + "/capsule_184x69.jpg", juegoBBDD.IDSteam, "ancha2")
                Catch ex As Exception

                End Try

                Dim imagenAncha As String = String.Empty

                Try
                    imagenAncha = Await Cache.DescargarImagen("https://steamcdn-a.akamaihd.net/steam/apps/" + juegoBBDD.IDSteam + "/header.jpg", juegoBBDD.IDSteam, "ancha")
                Catch ex As Exception

                End Try

                Dim imagenGrande As String = String.Empty

                Try
                    imagenGrande = Await Cache.DescargarImagen("https://steamcdn-a.akamaihd.net/steam/apps/" + juegoBBDD.IDSteam + "/library_600x900.jpg", juegoBBDD.IDSteam, "grande")
                Catch ex As Exception

                End Try

                If imagenGrande = String.Empty Then
                    Try
                        imagenGrande = Await Cache.DescargarImagen("https://steamcdn-a.akamaihd.net/steam/apps/" + juegoBBDD.IDSteam + "/capsule_616x353.jpg", juegoBBDD.IDSteam, "grande")
                    Catch ex As Exception

                    End Try
                End If

                Dim juego As New Tile(juegoBBDD.Titulo, juegoBBDD.IDBethesda, "bethesdanet://run/" + juegoBBDD.IDBethesda, imagenIcono, imagenLogo, imagenAnchaReducida, imagenAncha, imagenGrande)

                listaJuegos.Add(juego)
            End If

            tbProgreso.Text = i.ToString + "/" + listaBBDD.Count.ToString
            i += 1
        Next

        Await helper.SaveFileAsync(Of List(Of Tile))("juegos", listaJuegos)

        spProgreso.Visibility = Visibility.Collapsed

        Dim panelAvisoNoJuegos As Grid = pagina.FindName("panelAvisoNoJuegos")
        Dim gridSeleccionar As Grid = pagina.FindName("gridSeleccionarJuego")

        If listaJuegos.Count > 0 Then
            panelAvisoNoJuegos.Visibility = Visibility.Collapsed
            gridSeleccionar.Visibility = Visibility.Visible

            listaJuegos.Sort(Function(x, y) x.Titulo.CompareTo(y.Titulo))

            gv.Items.Clear()

            For Each juego In listaJuegos
                Dim boton As New Button

                Dim imagen As New ImageEx With {
                    .Source = juego.ImagenAncha,
                    .IsCacheEnabled = True,
                    .Stretch = Stretch.UniformToFill,
                    .Padding = New Thickness(0, 0, 0, 0)
                }

                boton.Tag = juego
                boton.Content = imagen
                boton.Padding = New Thickness(0, 0, 0, 0)
                boton.BorderThickness = New Thickness(1, 1, 1, 1)
                boton.BorderBrush = New SolidColorBrush(Colors.Black)
                boton.Background = New SolidColorBrush(Colors.Transparent)

                Dim tbToolTip As TextBlock = New TextBlock With {
                    .Text = juego.Titulo,
                    .FontSize = 16
                }

                ToolTipService.SetToolTip(boton, tbToolTip)
                ToolTipService.SetPlacement(boton, PlacementMode.Mouse)

                AddHandler boton.Click, AddressOf BotonTile_Click
                AddHandler boton.PointerEntered, AddressOf UsuarioEntraBoton
                AddHandler boton.PointerExited, AddressOf UsuarioSaleBoton

                gv.Items.Add(boton)
            Next
        Else
            panelAvisoNoJuegos.Visibility = Visibility.Visible
            gridSeleccionar.Visibility = Visibility.Collapsed
        End If

        botonCache.IsEnabled = True

    End Sub

    Private Sub BotonTile_Click(sender As Object, e As RoutedEventArgs)

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonJuego As Button = e.OriginalSource
        Dim juego As Tile = botonJuego.Tag

        Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
        botonAñadirTile.Tag = juego

        Dim imagenJuegoSeleccionado As ImageEx = pagina.FindName("imagenJuegoSeleccionado")
        imagenJuegoSeleccionado.Source = juego.ImagenAnchaReducida

        Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
        tbJuegoSeleccionado.Text = juego.Titulo

        Dim gridAñadir As Grid = pagina.FindName("gridAñadirTile")
        gridAñadir.Visibility = Visibility.Visible

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("tile", botonJuego)

        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("tile")

        If Not animacion Is Nothing Then
            animacion.TryStart(gridAñadir)
        End If

        Dim tbTitulo As TextBlock = pagina.FindName("tbTitulo")
        tbTitulo.Text = Package.Current.DisplayName + " (" + Package.Current.Id.Version.Major.ToString + "." + Package.Current.Id.Version.Minor.ToString + "." + Package.Current.Id.Version.Build.ToString + "." + Package.Current.Id.Version.Revision.ToString + ") - " + juego.Titulo

        '---------------------------------------------

        Dim titulo1 As TextBlock = pagina.FindName("tituloTileAnchaEnseñar")
        Dim titulo2 As TextBlock = pagina.FindName("tituloTileAnchaPersonalizar")

        Dim titulo3 As TextBlock = pagina.FindName("tituloTileGrandeEnseñar")
        Dim titulo4 As TextBlock = pagina.FindName("tituloTileGrandePersonalizar")

        titulo1.Text = juego.Titulo
        titulo2.Text = juego.Titulo

        titulo3.Text = juego.Titulo
        titulo4.Text = juego.Titulo

        If Not juego.ImagenIcono = Nothing Then
            Dim imagenPequeña1 As ImageEx = pagina.FindName("imagenTilePequeñaEnseñar")
            Dim imagenPequeña2 As ImageEx = pagina.FindName("imagenTilePequeñaGenerar")
            Dim imagenPequeña3 As ImageEx = pagina.FindName("imagenTilePequeñaPersonalizar")

            imagenPequeña1.Source = juego.ImagenIcono
            imagenPequeña2.Source = juego.ImagenIcono
            imagenPequeña3.Source = juego.ImagenIcono

            imagenPequeña1.Tag = juego.ImagenIcono
            imagenPequeña2.Tag = juego.ImagenIcono
            imagenPequeña3.Tag = juego.ImagenIcono
        End If

        If Not juego.ImagenAncha = Nothing Then
            Dim imagenMediana1 As ImageEx = pagina.FindName("imagenTileMedianaEnseñar")
            Dim imagenMediana2 As ImageEx = pagina.FindName("imagenTileMedianaGenerar")
            Dim imagenMediana3 As ImageEx = pagina.FindName("imagenTileMedianaPersonalizar")

            If Not juego.ImagenLogo = Nothing Then
                imagenMediana1.Source = juego.ImagenLogo
                imagenMediana2.Source = juego.ImagenLogo
                imagenMediana3.Source = juego.ImagenLogo

                imagenMediana1.Tag = juego.ImagenLogo
                imagenMediana2.Tag = juego.ImagenLogo
                imagenMediana3.Tag = juego.ImagenLogo
            Else
                imagenMediana1.Source = juego.ImagenAncha
                imagenMediana2.Source = juego.ImagenAncha
                imagenMediana3.Source = juego.ImagenAncha

                imagenMediana1.Tag = juego.ImagenAncha
                imagenMediana2.Tag = juego.ImagenAncha
                imagenMediana3.Tag = juego.ImagenAncha
            End If

            Dim imagenAncha1 As ImageEx = pagina.FindName("imagenTileAnchaEnseñar")
            Dim imagenAncha2 As ImageEx = pagina.FindName("imagenTileAnchaGenerar")
            Dim imagenAncha3 As ImageEx = pagina.FindName("imagenTileAnchaPersonalizar")

            imagenAncha1.Source = juego.ImagenAncha
            imagenAncha2.Source = juego.ImagenAncha
            imagenAncha3.Source = juego.ImagenAncha

            imagenAncha1.Tag = juego.ImagenAncha
            imagenAncha2.Tag = juego.ImagenAncha
            imagenAncha3.Tag = juego.ImagenAncha
        End If

        If Not juego.ImagenGrande = Nothing Then
            Dim imagenGrande1 As ImageEx = pagina.FindName("imagenTileGrandeEnseñar")
            Dim imagenGrande2 As ImageEx = pagina.FindName("imagenTileGrandeGenerar")
            Dim imagenGrande3 As ImageEx = pagina.FindName("imagenTileGrandePersonalizar")

            imagenGrande1.Source = juego.ImagenGrande
            imagenGrande2.Source = juego.ImagenGrande
            imagenGrande3.Source = juego.ImagenGrande

            imagenGrande1.Tag = juego.ImagenGrande
            imagenGrande2.Tag = juego.ImagenGrande
            imagenGrande3.Tag = juego.ImagenGrande
        End If

    End Sub

    Private Sub UsuarioEntraBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim imagen As ImageEx = boton.Content

        imagen.Saturation(0).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Hand, 1)

    End Sub

    Private Sub UsuarioSaleBoton(sender As Object, e As PointerRoutedEventArgs)

        Dim boton As Button = sender
        Dim imagen As ImageEx = boton.Content

        imagen.Saturation(1).Start()

        Window.Current.CoreWindow.PointerCursor = New CoreCursor(CoreCursorType.Arrow, 1)

    End Sub

    Public Async Function SacarIcono(id As String) As Task(Of String)

        Dim helper As New LocalObjectStorageHelper

        If Await helper.FileExistsAsync("juegos") = True Then
            Dim listaJuegos As List(Of Tile) = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")

            For Each juego In listaJuegos
                If id = juego.ID Then
                    If Not juego.ImagenIcono = Nothing Then
                        Return juego.ImagenIcono
                    End If
                End If
            Next
        End If

        Dim html As String = Await Decompiladores.HttpClient(New Uri("https://store.steampowered.com/app/" + id + "/"))
        Dim urlIcono As String = String.Empty

        If Not html = Nothing Then
            If html.Contains("<div class=" + ChrW(34) + "apphub_AppIcon") Then
                Dim temp, temp2 As String
                Dim int, int2 As Integer

                int = html.IndexOf("<div class=" + ChrW(34) + "apphub_AppIcon")
                temp = html.Remove(0, int)

                int = temp.IndexOf("<img src=")
                temp = temp.Remove(0, int + 10)

                int2 = temp.IndexOf(ChrW(34))
                temp2 = temp.Remove(int2, temp.Length - int2)

                temp2 = temp2.Replace("%CDN_HOST_MEDIA_SSL%", "steamcdn-a.akamaihd.net")

                urlIcono = temp2.Trim
            End If
        End If

        If urlIcono = Nothing Then
            html = Await Decompiladores.HttpClient(New Uri("https://steamdb.info/app/" + id + "/"))

            If Not html = Nothing Then
                If html.Contains("<img class=" + ChrW(34) + "app-icon avatar") Then
                    Dim temp, temp2 As String
                    Dim int, int2 As Integer

                    int = html.IndexOf("<img class=" + ChrW(34) + "app-icon avatar")
                    temp = html.Remove(0, int)

                    int = temp.IndexOf("src=")
                    temp = temp.Remove(0, int + 5)

                    int2 = temp.IndexOf(ChrW(34))
                    temp2 = temp.Remove(int2, temp.Length - int2)

                    urlIcono = temp2.Trim
                End If
            End If
        End If

        If Not urlIcono = String.Empty Then
            If Await helper.FileExistsAsync("juegos") = True Then
                Dim listaJuegos As List(Of Tile) = Await helper.ReadFileAsync(Of List(Of Tile))("juegos")

                For Each juego In listaJuegos
                    If id = juego.ID Then
                        juego.ImagenIcono = Await Cache.DescargarImagen(urlIcono, id, "icono")
                    End If
                Next

                Await helper.SaveFileAsync(Of List(Of Tile))("juegos", listaJuegos)
            End If
        End If

        Return urlIcono
    End Function

End Module

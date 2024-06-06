Imports MySql.Data.MySqlClient
Imports System.IO


Public Class FormPengaduanMasyarakat

	Sub kondisiAwal()
		PictureBox1.ImageLocation = ""
		RichTextBox1.Text = ""
	End Sub

	Sub autoNumeric()

		Dim urutanKode As String
		Dim hitung As Long
		Call koneksi()
		cmd = New MySqlCommand("SELECT * FROM pengaduan WHERE id_pengaduan IN (SELECT MAX(id_pengaduan) FROM pengaduan)", conn)
		dr = cmd.ExecuteReader
		dr.Read()

		If Not dr.HasRows Then

			urutanKode = "01"
			Label4.Text = urutanKode


		Else

			hitung = Microsoft.VisualBasic.Right(dr.GetInt32(0), 2) + 1
			urutanKode = Microsoft.VisualBasic.Right("0" & hitung, 1)
			Label4.Text = urutanKode

		End If

		dr.Close()
		cmd.Dispose()
		conn.Close()
	End Sub

	Sub cekNik()
		Call koneksi()
		cmd = New MySqlCommand("Select * from masyarakat where nama = '" & Label1.Text & "'", conn)
		dr = cmd.ExecuteReader
		dr.Read()
		If dr.HasRows Then
			Label5.Text = dr.Item("nik")
		End If

		dr.Close()
		cmd.Dispose()
		conn.Close()
	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		If RichTextBox1.Text = "" Then
			MsgBox("Harap isi pesan pengaduan nya terlebih dahulu")
		Else

			MsgBox("Anda yakin dengan pesan pengeaduan ini?", MsgBoxStyle.OkCancel)
			If MsgBoxResult.Ok Then
				autoNumeric()
				cekNik()
				Call koneksi()

				Dim simpanData As String = "INSERT INTO pengaduan(id_pengaduan, tgl_pengaduan, nik, isi_laporan, foto, lokasi, status) VALUES(
                                @idPengaduan,
                                @tglPengaduan,
                                @nik,
                                @isiLaporan,
                                @foto, @lokasi, 'proses')"

				cmd = New MySqlCommand(simpanData, conn)

				' Set parameter values
				cmd.Parameters.AddWithValue("@idPengaduan", Label4.Text)
				cmd.Parameters.AddWithValue("@tglPengaduan", DateTimePicker1.Value.ToString("yyyy-MM-dd"))
				cmd.Parameters.AddWithValue("@nik", Label5.Text)
				cmd.Parameters.AddWithValue("@isiLaporan", RichTextBox1.Text)

				' Convert image to byte array and set it as a parameter
				Dim fotoArray As Byte() = File.ReadAllBytes(Label2.Text)
				cmd.Parameters.AddWithValue("@foto", fotoArray)

				cmd.Parameters.AddWithValue("@lokasi", TextBox1.Text)

				cmd.ExecuteNonQuery()
				MsgBox("Pesan pengaduan berhasil dikirim", MsgBoxStyle.Information)
				kondisiAwal()
			End If

		End If
	End Sub

	Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

		If MsgBox("Anda yakin ingin keluar menu ini?", MsgBoxStyle.YesNo, "Peringatan!") = MsgBoxResult.Yes Then

			Me.Hide()
			MainMenuMasyarakat.Show()

		End If

	End Sub

	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

		If FilePhoto.ShowDialog() = DialogResult.OK Then
			FilePhoto.Filter = ".jpg |"
			Label2.Text = FilePhoto.FileName
			PictureBox1.ImageLocation = Label2.Text
			PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
		End If

	End Sub

	Private Sub FormPengaduanMasyarakat_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Dim msg As String = "Anda yakin ingin keluar dari aplikasi ini?"
		Dim title As String = "Form closing"
		Dim result = MessageBox.Show(msg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
		If result = DialogResult.Cancel Then
			e.Cancel = True
		Else
			MainMenuMasyarakat.Show()
		End If
	End Sub

	Private Sub FormPengaduanMasyarakat_Load(sender As Object, e As EventArgs) Handles MyBase.Load

	End Sub

	Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

	End Sub
End Class
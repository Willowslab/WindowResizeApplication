Public Class previewwindow

    Private Sub title_Click(sender As Object, e As EventArgs) Handles Me.Click, Panel1.Click, Panel2.Click, Panel3.Click
        Me.Close()
    End Sub

    Private Sub txch() Handles Me.TextChanged
        Me.title.Text = Me.Text & " (Press to Close)"
    End Sub
End Class
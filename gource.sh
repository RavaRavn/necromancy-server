# https://gource.io/
# brew install gource
# brew install libav
gource \
-s .7 \
-1920x1080 \
--auto-skip-seconds .1 \
--multi-sampling \
--stop-at-end \
--highlight-users \
--hide mouse,progress \
--file-idle-time 13 \
--max-files 0 \
--background-colour 000000 \
--font-size 26 \
--output-framerate 30 \
--output-ppm-stream - \
| avconv -y -r 30 -f image2pipe -vcodec ppm -i - -b 32768k movie.mp4

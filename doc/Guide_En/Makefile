PANDOC_OPT_PDF= --toc-depth=2 -N --table-of-contents -f markdown+autolink_bare_uris \
                --listings \
                --template=eisvogel.tex \
                -V documentclass=ltjsarticle \
                -V classoption=titlepage \
                --pdf-engine=xelatex

all: FlexConfirmMailGuideEn.pdf

%.pdf: %.md
	pandoc ${PANDOC_OPT_PDF} -o $@ < $^

clean:
	rm -f *.pdf

.PHONY: all clean
